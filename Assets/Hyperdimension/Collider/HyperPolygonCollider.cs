using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension/Collider/Polygon Collider")]
    public class HyperPolygonCollider : HyperBaseCollider
    {
        float radius;

        [SerializeField]
        float height;

        [SerializeField]
        Vector2[] vertices;
        

        public override float Radius { get { return radius; } set { SetRadius(); } }

        public override float Height { get { return height; } set { height = value; } }

        public Vector2[] Vertices { get { return vertices; } set { vertices = value; SetRadius(); } }


        protected override void Awake()
        {
            SetRadius();
            
            base.Awake();
        }

        protected override void Start()
        {
            if (height > 0f)
                base.Start();
        }


        void SetRadius()
        {
            if (vertices == null) return;
            
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

        public float GetLeastRadius()
        {
            if (vertices == null) return 0f;
            
            float radius = 0f;

            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2 rotatedVertices = Math.RotatedVertex(Vertices[i], HyperTransform.Angle);
                float distance = Math.Distance(0f, 0f, rotatedVertices.x, 0f);

                if (radius < distance)
                    radius = distance;
            }

            return radius;
        }
    }
}
