using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension/Physics/Physics Manager")]
    public class HyperPhysicsManager : MonoBehaviour
    {
        HyperZone zone;

        List<HyperBaseCollider> colliders = new List<HyperBaseCollider>();

        [SerializeField]
        Vector2 zoneCenter = Vector2.zero;
        [SerializeField]
        int zoneSizeX = 100;
        [SerializeField]
        int zoneSizeY = 100;
        [SerializeField]
        float zoneCellSize = 2f;

        [SerializeField] 
        bool collisionResolve = true;
        [SerializeField]
        float collisionResolveFactor = 1f;
        
        
        public Vector2 ZoneCenter { get { return zoneCenter; } set { zoneCenter = value; } }
        public int ZoneSizeX { get { return zoneSizeX; } set { zoneSizeX = value; } }
        public int ZoneSizeY { get { return zoneSizeY; } set { zoneSizeY = value; } }
        public float ZoneCellSize { get { return zoneCellSize; } set { zoneCellSize = value; } }
        
        public HyperZone Zone { get { return zone; } }
        public List<HyperBaseCollider> Colliders { get { return colliders; } }
        
        public bool CollisionResolve { get { return collisionResolve;} set { collisionResolve = value; } }
        public float CollisionResolveFactor { get { return collisionResolveFactor;} set { collisionResolveFactor = value; } }
        
        
        private void Awake()
        {
            zone = new HyperZone(zoneCenter.x, zoneCenter.y, zoneSizeX, zoneSizeY, zoneCellSize);
        }

        private void Update()
        {
            CalculateColliderZone();
        }

        private void FixedUpdate()
        {
            if (collisionResolve)
                CollisionResolution();
        }

        void CalculateColliderZone()
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (!colliders[i].isActiveAndEnabled)
                {
                    if (colliders[i].OverlappedZoneCells.Count > 0)
                    {
                        for (int j = 0; j < colliders[i].OverlappedZoneCells.Count; j++)
                        {
                            colliders[i].OverlappedZoneCells[j].colliders.Remove(colliders[i]);
                        }

                        colliders[i].OverlappedZoneCells.Clear();
                    }

                    continue;
                }
                
                int xIndex = 0;
                int yIndex = 0;

                HyperZoneCell[] overlappedZoneCells = zone.GetOverlappedZoneCells(colliders[i].X, colliders[i].Y, colliders[i].Radius, ref xIndex, ref yIndex);
                
                if (colliders[i].OverlappedZoneCells.Count != overlappedZoneCells.Length || colliders[i].CurrentZoneCell != zone.zoneCells[xIndex][yIndex])
                {
                    colliders[i].CurrentZoneCell = zone.zoneCells[xIndex][yIndex];

                    for (int j = 0; j < colliders[i].OverlappedZoneCells.Count; j++)
                    {
                        colliders[i].OverlappedZoneCells[j].colliders.Remove(colliders[i]);
                    }

                    colliders[i].OverlappedZoneCells.Clear();

                    for (int j = 0; j < overlappedZoneCells.Length; j++)
                    {
                        colliders[i].OverlappedZoneCells.Add(overlappedZoneCells[j]);
                        overlappedZoneCells[j].colliders.Add(colliders[i]);
                    }
                }
            }
        }

        void CollisionResolution()
        {
            for (int i = 0; i < zone.zoneCells.Count; i++)
            {
                for (int j = 0; j < zone.zoneCells[i].Count; j++)
                {
                    for (int k = 0; k < zone.zoneCells[i][j].colliders.Count; k++)
                    {
                        if (!zone.zoneCells[i][j].colliders[k].isActiveAndEnabled)
                            continue;

                        if (zone.zoneCells[i][j].colliders[k].IsFixed)
                            continue;
                        
                        for (int l = 0; l < zone.zoneCells[i][j].colliders.Count; l++)
                        {
                            if (k == l)
                                continue;

                            HyperBaseCollider collider1 = zone.zoneCells[i][j].colliders[k];
                            HyperBaseCollider collider2 = zone.zoneCells[i][j].colliders[l];

                            if (HyperPhysics.IsCollideWith(collider1, collider2))
                            {
                                if (collider2.GetType() == typeof(HyperPlaneCollider))
                                {
                                    HyperPlaneCollider planeCollider = (HyperPlaneCollider)collider2;
                                    collider1.Z = planeCollider.GetZValue(collider1.X, collider1.Y);
                                }
                                else
                                {
                                    Vector2 delta = collider1.Position - collider2.Position;
                                    delta = delta.normalized * Time.fixedDeltaTime * collisionResolveFactor;

                                    collider1.HyperTransform.Translate(delta);
                                    
                                    if (!collider2.IsFixed)
                                        collider2.HyperTransform.Translate(-delta);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}