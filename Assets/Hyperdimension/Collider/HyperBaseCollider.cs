using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public abstract class HyperBaseCollider : MonoBehaviour
    {
        HyperBaseTransform hyperTransform;

        [SerializeField]
        bool isFixed = false;

        HyperZoneCell currentZoneCell = null;

        List<HyperZoneCell> overlappedZoneCells = new List<HyperZoneCell>();


        public HyperBaseTransform HyperTransform { get { if (hyperTransform == null) hyperTransform = GetComponent<HyperBaseTransform>(); return hyperTransform; } }

        public bool IsFixed { get { return isFixed; } set { isFixed = value; } }
        
        public HyperZoneCell CurrentZoneCell { get { return currentZoneCell; } set { currentZoneCell = value; } }
        
        public List<HyperZoneCell> OverlappedZoneCells { get { return overlappedZoneCells; } }
        
        public abstract float Radius { get; set; }

        public abstract float Height { get; set; }

        
        protected virtual void Awake() { }
        protected virtual void Start()
        {
            HyperPhysicsManager physicsManager = FindObjectOfType<HyperPhysicsManager>();
            if (physicsManager != null)
            {
                physicsManager.Colliders.Add(this);
            }
        }

        public virtual bool RaycastToThis(HyperRay ray)
        {
            HyperRaycastHit raycastHit;
            return RaycastToThis(ray, out raycastHit);
        }

        public virtual bool RaycastToThis(HyperRay ray, out HyperRaycastHit raycastHit)
        {
            return HyperPhysics.Raycast(ray, this, out raycastHit);
        }
        
        public virtual bool RaycastFromThis(Vector3 direction, float maxDistance)
        {
            HyperRaycastHit raycastHit;
            return RaycastFromThis(direction, maxDistance, out raycastHit);
        }

        public virtual bool RaycastFromThis(Vector3 direction, float maxDistance, out HyperRaycastHit raycastHit)
        {
            return HyperPhysics.Raycast(new HyperRay(this.HyperTransform.Position, direction, maxDistance), out raycastHit, exceptThis: this);
        }
        
        public virtual bool IsCollideWith(HyperBaseCollider givenCollider)
        {
            return HyperPhysics.IsCollideWith(this, givenCollider);
        }
    }
}
