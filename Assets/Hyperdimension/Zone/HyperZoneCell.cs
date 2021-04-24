using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    [Serializable]
    public class HyperZoneCell
    {
        Vector2[] vertices;


        public Vector2[] Vertices { get { return vertices; } set { vertices = value; } }

        public float X1 { get { return vertices[0].x; } }
        
        public float X2 { get { return vertices[1].x; } }
        
        public float Y1 { get { return vertices[1].y; } }
        
        public float Y2 { get { return vertices[2].y; } }

        
        public List<HyperBaseCollider> colliders;

        public HyperZoneCell()
        {
            colliders = new List<HyperBaseCollider>();
        }

        public bool IsInZone(float x, float y, float radius)
        {
            if (radius <= X2 - X1)
            {
                float leftX = x - radius;
                float rightX = x + radius;
                float downY = y - radius;
                float upY = y + radius;

                if (x >= X1 && x <= X2 && y >= Y1 && y <= Y2)
                    return true;
                else
                {
                    bool leftCheck = (leftX >= X1 && leftX <= X2);
                    bool rightCheck = (rightX >= X1 && rightX <= X2);
                    bool downCheck = (downY >= Y1 && downY <= Y2);
                    bool upCheck = (upY >= Y1 && upY <= Y2);

                    if (y >= Y1 && y <= Y2 && (leftCheck || rightCheck))
                        return true;
                    else if (x >= X1 && x <= X2 && (downCheck || upCheck))
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
