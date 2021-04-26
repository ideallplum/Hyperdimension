using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public class HyperZone
    {
        float xOrigin;
        float yOrigin;

        [SerializeField]
        float xCenter;
        [SerializeField]
        float yCenter;

        [SerializeField]
        int xSize;
        [SerializeField]
        int ySize;

        [SerializeField]
        float zoneCellSize;

        public float XCenter { get { return xCenter; } set { xCenter = value; CalculateXOrigin(); ResetZone(); } }
        public float YCenter { get { return yCenter; } set { yCenter = value; CalculateYOrigin(); ResetZone(); } }

        public float XOrigin { get { return xOrigin; } }
        public float YOrigin { get { return yOrigin; } }

        public int XSize { get { return xSize; } set { xSize = value; ResetZone(); } }
        public int YSize { get { return ySize; } set { ySize = value; ResetZone(); } }
        public float ZoneCellSize { get { return zoneCellSize; } set { zoneCellSize = value; ResetZone(); } }

        public List<List<HyperZoneCell>> zoneCells;

        public HyperZone(float xCenter, float yCenter, int xSize, int ySize, float zoneCellSize)
        {
            this.xSize = xSize;
            this.ySize = ySize;
            this.zoneCellSize = zoneCellSize;

            this.xCenter = xCenter;
            this.yCenter = yCenter;

            CalculateXOrigin();
            CalculateYOrigin();

            SetZone();
        }


        public HyperZoneCell[] GetOverlappedZoneCells(float x, float y, float radius, ref int xIndex, ref int yIndex)
        {
            List<HyperZoneCell> overlappedZones = new List<HyperZoneCell>();

            xIndex = Mathf.FloorToInt((x - XOrigin) / ZoneCellSize);
            yIndex = Mathf.FloorToInt((y - YOrigin) / ZoneCellSize);

            if (xIndex < 0) xIndex = 0;
            if (yIndex < 0) yIndex = 0;
            if (xIndex >= xSize) xIndex = xSize - 1;
            if (yIndex >= ySize) yIndex = ySize - 1;

            int extendCount = Mathf.CeilToInt(Mathf.Ceil(radius / (ZoneCellSize * 0.5f)) / 2f);

            for (int j = xIndex - extendCount; j <= xIndex + extendCount; j++)
            {
                for (int k = yIndex - extendCount; k <= yIndex + extendCount; k++)
                {
                    if (j < 0 || j >= XSize || k < 0 || k >= YSize)
                        continue;

                    if (zoneCells[j][k].IsInZone(x, y, radius))
                    {
                        overlappedZones.Add(zoneCells[j][k]);
                    }
                }
            }

            return overlappedZones.ToArray();
        }

        public HyperBaseCollider[] GetValidColliders(float x, float y, float radius)
        {
            List<HyperBaseCollider> colliderList = new List<HyperBaseCollider>();

            int xIndex = 0;
            int yIndex = 0;

            HyperZoneCell[] overlappedZoneCells = GetOverlappedZoneCells(x, y, radius, ref xIndex, ref yIndex);

            for (int i = 0; i < overlappedZoneCells.Length; i++)
            {
                for (int j = 0; j < overlappedZoneCells[i].colliders.Count; j++)
                {
                    if (overlappedZoneCells[i].colliders[j].isActiveAndEnabled == false)
                        continue;
                    
                    if (!colliderList.Contains(overlappedZoneCells[i].colliders[j]))
                        colliderList.Add(overlappedZoneCells[i].colliders[j]);
                }
            }

            return colliderList.ToArray();
        }

        void SetZone()
        {
            zoneCells = new List<List<HyperZoneCell>>();

            for (int i = 0; i < XSize; i++)
            {
                zoneCells.Add(new List<HyperZoneCell>());

                for (int j = 0; j < YSize; j++)
                {
                    zoneCells[i].Add(new HyperZoneCell());
                    zoneCells[i][j].Vertices = new Vector2[] 
                        { 
                            new Vector2(xOrigin + (i * zoneCellSize), yOrigin + (j * zoneCellSize)), 
                            new Vector2(xOrigin + ((i + 1) * zoneCellSize), yOrigin + (j * zoneCellSize)), 
                            new Vector2(xOrigin + ((i + 1) * zoneCellSize), yOrigin + ((j + 1) * zoneCellSize)), 
                            new Vector2(xOrigin + (i * zoneCellSize), yOrigin + ((j + 1) * zoneCellSize)) 
                        };
                }
            }
        }
        void ResetZone()
        {
            for (int i = 0; i < zoneCells.Count; i++)
            {
                zoneCells[i].Clear();
            }
            zoneCells.Clear();

            SetZone();
        }

        void CalculateXOrigin()
        {
            xOrigin = xCenter - (xSize * zoneCellSize * 0.5f);
        }

        void CalculateYOrigin()
        {
            yOrigin = yCenter - (ySize * zoneCellSize * 0.5f);
        }
    }
}
