using UnityEngine;

namespace Hyperdimension
{
    public class HyperRay
    {
        public Vector3 From { get; set; }

        public Vector3 To { get; set; }

        public Vector3 Direction { get { return (To - From).normalized; } }
        
        public Vector3 Center { get { return Vector3.Lerp(From, To, 0.5f); } }

        public float Length { get { return (To - From).magnitude; } }

        public float Radius { get { return Length * 0.5f; } }
        

        public HyperRay(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
        }

        public HyperRay(Vector3 origin, Vector3 direction, float maxDistance)
        {
            From = origin;
            To = origin + (direction.normalized * maxDistance);
        }
    }
}