using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public class Zone
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

        public List<List<ZoneCell>> zoneCells;

        public Zone(float xCenter, float yCenter, int xSize, int ySize, float zoneCellSize)
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


        public ZoneCell[] GetOverlappedZoneCells(float x, float y, float radius, ref int xIndex, ref int yIndex)
        {
            List<ZoneCell> overlappedZones = new List<ZoneCell>();

            xIndex = Mathf.FloorToInt((x - XOrigin) / ZoneCellSize);
            yIndex = Mathf.FloorToInt((y - YOrigin) / ZoneCellSize);

            int zoneCount = 0;

            int extendCount = Mathf.CeilToInt(Mathf.Ceil(radius / (ZoneCellSize * 0.5f)) / 2f);

            for (int j = xIndex - extendCount; j <= xIndex + extendCount; j++)
            {
                for (int k = yIndex - extendCount; k <= yIndex + extendCount; k++)
                {
                    if (j < 0 || j >= XSize || k < 0 || k >= YSize)
                        continue;

                    if (zoneCells[j][k].IsInZone(x, y, radius))
                    {
                        zoneCount++;
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

            ZoneCell[] zoneCells = GetOverlappedZoneCells(x, y, radius, ref xIndex, ref yIndex);

            for (int i = 0; i < zoneCells.Length; i++)
            {
                for (int j = 0; j < zoneCells[i].colliders.Count; j++)
                {
                    if (!colliderList.Contains(zoneCells[i].colliders[j]))
                        colliderList.Add(zoneCells[i].colliders[j]);
                }
            }

            return colliderList.ToArray();
        }

        void SetZone()
        {
            zoneCells = new List<List<ZoneCell>>();

            for (int i = 0; i < XSize; i++)
            {
                zoneCells.Add(new List<ZoneCell>());

                for (int j = 0; j < YSize; j++)
                {
                    zoneCells[i].Add(new ZoneCell());
                    Vector2[] zoneVertices = new Vector2[] { new Vector2(xOrigin + (i * zoneCellSize), yOrigin + (j * zoneCellSize)), new Vector2(xOrigin + ((i + 1) * zoneCellSize), yOrigin + (j * zoneCellSize)), new Vector2(xOrigin + ((i + 1) * zoneCellSize), yOrigin + ((j + 1) * zoneCellSize)), new Vector2(xOrigin + (i * zoneCellSize), yOrigin + ((j + 1) * zoneCellSize)) };
                    zoneCells[i][j].x1 = zoneVertices[0].x;
                    zoneCells[i][j].x2 = zoneVertices[1].x;
                    zoneCells[i][j].y1 = zoneVertices[1].y;
                    zoneCells[i][j].y2 = zoneVertices[2].y;
                    zoneCells[i][j].vertices = zoneVertices;
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
