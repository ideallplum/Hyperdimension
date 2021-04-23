using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    [Serializable]
    public class ZoneCell
    {
        public Vector2[] vertices;

        public float x1;
        public float x2;
        public float y1;
        public float y2;

        public List<HyperBaseCollider> colliders;

        public ZoneCell()
        {
            colliders = new List<HyperBaseCollider>();
        }

        public bool IsInZone(float x, float y, float radius)
        {
            if (radius <= x2 - x1)
            {
                float leftX = x - radius;
                float rightX = x + radius;
                float downY = y - radius;
                float upY = y + radius;

                if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                    return true;
                else
                {
                    bool leftCheck = (leftX >= x1 && leftX <= x2);
                    bool rightCheck = (rightX >= x1 && rightX <= x2);
                    bool downCheck = (downY >= y1 && downY <= y2);
                    bool upCheck = (upY >= y1 && upY <= y2);

                    if (y >= y1 && y <= y2 && (leftCheck || rightCheck))
                        return true;
                    else if (x >= x1 && x <= x2 && (downCheck || upCheck))
                        return true;
                    else if ((leftCheck && upCheck) || (leftCheck && downCheck) || (rightCheck && upCheck) || (rightCheck && downCheck))
                        return true;
                }
            }
            else
            {
                if (Math.PolygonCircle(vertices, x, y, radius))
                    return true;
            }
            return false;
        }
    }
}
