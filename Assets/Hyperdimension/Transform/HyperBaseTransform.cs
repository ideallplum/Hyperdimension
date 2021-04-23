using UnityEngine;

namespace Hyperdimension
{
    public abstract class HyperBaseTransform : MonoBehaviour
    {
        [SerializeField]
        Vector3 position;
        
        [SerializeField]
        float angle = 0f;

        [SerializeField]
        int sortingOrderOffset = 0;

        protected bool isMoved = false;

        
        public float X { get { return position.x; }set { position = new Vector3(value, position.y, position.z); isMoved = true; } }

        public float Y { get { return position.y; } set { position = new Vector3(position.x, value, position.z); isMoved = true; } }

        public float Z { get { return position.z; } set { position = new Vector3(position.x, position.y, value); isMoved = true; } }

        public Vector3 Position { get { return position; } set { position = value; isMoved = true; } }

        public float Angle { get { return angle; } set { angle = value; } }

        public int SortingOrderOffset { get { return sortingOrderOffset; } set { sortingOrderOffset = value; } }


        protected void UpdatePosition()
        {
            gameObject.transform.localPosition = Math.SpaceToWorld(position);
        }

        public void Translate(Vector3 translation)
        {
            Position += translation;
            isMoved = true;
        }

        public static int GetSortingOrder(float Y, int sortingOrderOffset)
        {
            return -(int)(Y * Settings.pixelsPerUnit) + sortingOrderOffset;
        }
    }
}