using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension//Collider/Line Collider")]
    public class HyperLineCollider : HyperBaseCollider
    {
        float radius;

        [SerializeField]
        Vector3 from;

        [SerializeField]
        Vector3 to;


        public override float Radius { get { return radius; } set { SetRadius(); } }

        public override float Height { get { return 0f; } set { } }

        public Vector3 From { get { return from; } set { from = value; SetRadius(); } }

        public Vector3 To { get { return to; } set { to = value; SetRadius(); } }


        private void Awake()
        {
            SetRadius();
        }

        void SetRadius()
        {
            if (from != null && to != null)
            {
                radius = (to - from).magnitude * 0.5f;
            }
        }
    }
}

