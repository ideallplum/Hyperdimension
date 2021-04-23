using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension//Collider/Plane Collider")]
    public class HyperPlaneCollider : HyperBaseCollider
    {
        float radius;

        float height;

        [SerializeField]
        Vector3[] vertices;

        [SerializeField]
        bool isSolid;


        public override float Radius { get { return radius; } set { SetRadius(); } }

        public override float Height { get { return height; } set { SetHeight(); } }

        public Vector3[] Vertices { get { return vertices; } set { vertices = value; CheckNormal(); SetRadius(); } }

        public bool IsSolid { get { return isSolid; } set { isSolid = value; } }

        public bool IsFlat
        {
            get
            {
                for (int i = 0; i < Vertices.Length; i++)
                {
                    if (Vertices[i].z != 0)
                        return false;
                }

                return true;
            }
        }


        private void Awake()
        {
            CheckNormal();
            SetRadius();
            SetHeight();
        }


        void CheckNormal()
        {
            if (Vertices != null)
            {
                if (Vertices.Length > 1)
                {
                    Vector3 normal = Vector3.Cross(Vertices[0], Vertices[1]);

                    if (normal.z < 0f)
                    {
                        List<Vector3> tempList = new List<Vector3>(Vertices);
                        tempList.Reverse();

                        Vertices = tempList.ToArray();
                    }
                }
            }
        }

        public Vector2[] GetFlatPolygon()
        {
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < Vertices.Length; i++)
            {
                list.Add(new Vector2(Vertices[i].x, Vertices[i].y));
            }

            return list.ToArray();
        }

        public void SetHeight()
        {
            float height = 0f;

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Vertices[i].z > height)
                    height = Vertices[i].z;
            }

            this.height = height;
        }

        public void SetRadius()
        {
            float radius = 0f;

            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2 rotatedVertices = Math.RotatedVertex(Vertices[i], HyperTransform.Angle);
                float distance = Math.Distance(0f, 0f, rotatedVertices.x, rotatedVertices.y);

                if (radius < distance)
                    radius = distance;
            }

            this.radius = radius;
        }

        public float GetZValue(float x, float y)
        {
            Vector2 centerDirection = (new Vector2(HyperTransform.X, HyperTransform.Y) - new Vector2(x, y)).normalized;
            centerDirection = centerDirection * 0.000001f;
            x += centerDirection.x;
            y += centerDirection.y;

            HyperRaycastHit raycastHit;
            HyperRay ray = new HyperRay(new Vector3(x, y, HyperTransform.Z + Height + 0.01f), new Vector3(x, y, HyperTransform.Z - 0.01f));
            if (Raycast(ray, out raycastHit))
                return raycastHit.point.z;

            return 0f;
        }
    }
}
