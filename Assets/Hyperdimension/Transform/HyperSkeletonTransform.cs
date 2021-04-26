using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension/Transform/Skeleton Transform")]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class HyperSkeletonTransform : HyperBaseTransform
    {
        MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            isMoved = true;
#endif

            if (isMoved)
            {
                UpdatePosition();
                meshRenderer.sortingOrder = GetSortingOrder(Y, SortingOrderOffset);
                isMoved = false;
            }

        }
    }
}
