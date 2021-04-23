using UnityEngine;

namespace Hyperdimension
{
    public class HyperRaycastHit
    {
        public HyperBaseCollider collider;
        public float distance;
        public Vector3 point;
        public HyperBaseTransform hyperTransform;


        public HyperRaycastHit() { }

        public HyperRaycastHit(HyperBaseCollider collider, float distance, Vector3 point, HyperBaseTransform hyperTransform)
        {
            this.collider = collider;
            this.distance = distance;
            this.point = point;
            this.hyperTransform = hyperTransform;
        }
    }
}
