using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public abstract class HyperBaseCollider : MonoBehaviour
    {
        HyperBaseTransform hyperTransform;

        [SerializeField]
        bool isFixed = false;

        [SerializeField]
        Vector3 offset;
        
        HyperZoneCell currentZoneCell = null;

        List<HyperZoneCell> overlappedZoneCells = new List<HyperZoneCell>();


        public HyperBaseTransform HyperTransform { get { if (hyperTransform == null) hyperTransform = GetComponent<HyperBaseTransform>(); return hyperTransform; } }

        public bool IsFixed { get { return isFixed; } set { isFixed = value; } }
        
        public float OffsetX { get { return offset.x; } set { offset = new Vector3(value, offset.y, offset.z); } }
        public float OffsetY { get { return offset.y; } set { offset = new Vector3(offset.x, value, offset.z); } }
        public float OffsetZ { get { return offset.z; } set { offset = new Vector3(offset.x, offset.y, value); } }
        public Vector3 Offset { get { return offset; } set { offset = value; } }
        
        public float X { get { return HyperTransform.X + Math.RotatedVertex(new Vector2(offset.x, offset.y), Angle).x; } }
        public float Y { get { return HyperTransform.Y + Math.RotatedVertex(new Vector2(offset.x, offset.y), Angle).y; } }
        public float Z { get { return HyperTransform.Z + OffsetZ; } set {HyperTransform.Z = value - OffsetZ; }}
        public Vector3 Position { get { Vector2 rotatedXY = Math.RotatedVertex(new Vector2(offset.x, offset.y), Angle); return HyperTransform.Position + new Vector3(rotatedXY.x, rotatedXY.y, OffsetZ); } }
        public float Angle { get { return HyperTransform.Angle; } }
        
        
        public HyperZoneCell CurrentZoneCell { get { return currentZoneCell; } set { currentZoneCell = value; } }
        
        public List<HyperZoneCell> OverlappedZoneCells { get { return overlappedZoneCells; } }
        
        public abstract float Radius { get; set; }

        public abstract float Height { get; set; }

        
        protected virtual void Awake() { }
        protected virtual void Start()
        {
            HyperPhysicsManager physicsManager = FindObjectOfType<HyperPhysicsManager>();
            if (physicsManager != null)
            {
                physicsManager.Colliders.Add(this);
            }
        }

        public virtual bool RaycastToThis(HyperRay ray)
        {
            HyperRaycastHit raycastHit;
            return RaycastToThis(ray, out raycastHit);
        }

        public virtual bool RaycastToThis(HyperRay ray, out HyperRaycastHit raycastHit)
        {
            return HyperPhysics.Raycast(ray, this, out raycastHit);
        }
        
        public virtual bool RaycastFromThis(Vector3 direction, float maxDistance)
        {
            HyperRaycastHit raycastHit;
            return RaycastFromThis(direction, maxDistance, out raycastHit);
        }

        public virtual bool RaycastFromThis(Vector3 direction, float maxDistance, out HyperRaycastHit raycastHit)
        {
            return HyperPhysics.Raycast(new HyperRay(this.HyperTransform.Position, direction, maxDistance), out raycastHit, exceptThis: this);
        }
        
        public virtual bool IsCollideWith(HyperBaseCollider givenCollider)
        {
            return HyperPhysics.IsCollideWith(this, givenCollider);
        }
    }
}
