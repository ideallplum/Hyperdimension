using UnityEngine;

namespace Hyperdimension
{
    [AddComponentMenu("Hyperdimension/Transform/Sprite Transform")]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class HyperSpriteTransform : HyperBaseTransform
    {
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
#if UNITY_EDITOR
            isMoved = true;
#endif

            if (isMoved)
            {
                UpdatePosition();
                spriteRenderer.sortingOrder = GetSortingOrder(Y, SortingOrderOffset);
                isMoved = false;
            }
        }

    }
}