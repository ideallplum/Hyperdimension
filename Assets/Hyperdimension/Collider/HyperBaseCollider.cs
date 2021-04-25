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


        private void Start()
        {
            HyperPhysicsManager physicsManager = FindObjectOfType<HyperPhysicsManager>();
            if (physicsManager != null)
            {
                physicsManager.Colliders.Add(this);
            }
        }

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
