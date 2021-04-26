using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension/Collider/Cylinder Collider")]
    public class HyperCylinderCollider : HyperBaseCollider
    {
        [SerializeField]
        float radius;

        [SerializeField]
        float height;


        public override float Radius { get { return radius; } set { radius = value; } }

        public override float Height { get { return height; } set { height = value; } }


        protected override void Start()
        {
            if (height > 0f)
                base.Start();
        }
    }
}
