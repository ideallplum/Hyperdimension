using UnityEngine;

namespace Hyperdimension
{
    public abstract class HyperBaseCollider : MonoBehaviour
    {
        HyperBaseTransform hyperTransform;


        public HyperBaseTransform HyperTransform { get { if (hyperTransform == null) hyperTransform = GetComponent<HyperBaseTransform>(); return hyperTransform; } }

        public abstract float Radius { get; set; }

        public abstract float Height { get; set; }


        public virtual bool Raycast(HyperRay ray)
        {
            HyperRaycastHit raycastHit;
            return Raycast(ray, out raycastHit);
        }

        public virtual bool Raycast(HyperRay ray, out HyperRaycastHit raycastHit)
        {
            return HyperPhysics.Raycast(ray, this, out raycastHit);
        }
    }
}
