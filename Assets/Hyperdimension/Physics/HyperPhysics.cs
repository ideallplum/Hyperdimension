using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public class HyperPhysics
    {
        public static bool IsCollideWith(HyperBaseCollider collider1, HyperBaseCollider collider2)
        {
            bool collision = false;

            if (Math.IsCollideZ(collider1.HyperTransform.Z, collider1.Height, collider2.HyperTransform.Z, collider2.Height))
            {
                Type type1 = collider1.GetType();
                Type type2 = collider2.GetType();

                if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperCylinderCollider hyperCylinderCollider2 = (HyperCylinderCollider)collider2;
                    collision = Math.CircleCircle(hyperCylinderCollider1.HyperTransform.X, hyperCylinderCollider1.HyperTransform.Y, hyperCylinderCollider1.Radius, hyperCylinderCollider2.HyperTransform.X, hyperCylinderCollider2.HyperTransform.Y, hyperCylinderCollider2.Radius);
                }
                else if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.HyperTransform.Angle), hyperCylinderCollider1.HyperTransform.X, hyperCylinderCollider1.HyperTransform.Y, hyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider2;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider1;
                    collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.HyperTransform.Angle), hyperCylinderCollider1.HyperTransform.X, hyperCylinderCollider1.HyperTransform.Y, hyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperPolygonCollider hyperPolygonCollider1 = (HyperPolygonCollider)collider1;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonPolygon(Math.TransformedVertices(hyperPolygonCollider1.Vertices, hyperPolygonCollider1.HyperTransform.Position, hyperPolygonCollider1.HyperTransform.Angle), Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.HyperTransform.Angle));
                }
                else if (type1 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider1;
                    if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.HyperTransform.Angle), collider2.HyperTransform.X, collider2.HyperTransform.Y, Mathf.Epsilon))
                    {
                        collision = collider2.HyperTransform.Z <= hyperPlaneCollider.GetZValue(collider2.HyperTransform.X, collider2.HyperTransform.Y) ? true : false;
                    }
                }
                else if (type2 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider2;
                    if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.HyperTransform.Angle), collider1.HyperTransform.X, collider1.HyperTransform.Y, Mathf.Epsilon))
                    {
                        collision = collider1.HyperTransform.Z <= hyperPlaneCollider.GetZValue(collider1.HyperTransform.X, collider1.HyperTransform.Y) ? true : false;
                    }
                }
                else if (type1 == typeof(HyperLineCollider))
                {
                    HyperLineCollider hyperLineCollider = (HyperLineCollider)collider1;
                    HyperRaycastHit hyperRaycastHit;

                    if (Raycast(new HyperRay(hyperLineCollider.From, hyperLineCollider.To), collider2, out hyperRaycastHit))
                    {
                        collision = true;
                    }
                }
                else if (type2 == typeof(HyperLineCollider))
                {
                    HyperLineCollider hyperLineCollider = (HyperLineCollider)collider2;
                    HyperRaycastHit hyperRaycastHit;

                    if (Raycast(new HyperRay(hyperLineCollider.From, hyperLineCollider.To), collider1, out hyperRaycastHit))
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }


        public static bool Raycast(HyperRay ray, HyperBaseCollider collider, out HyperRaycastHit hyperRaycastHit)
        {
            hyperRaycastHit = new HyperRaycastHit(null, -1f, Vector3.zero, null);

            Type type = collider.GetType();

            Vector2 interSection1 = Vector2.zero;
            Vector2 interSection2 = Vector2.zero;
            Vector3 realPoint = Vector3.zero;
            int result = 0;

            if (type == typeof(HyperLineCollider))
            {
                return false;
            }
            else if (type == typeof(HyperCylinderCollider))
            {
                HyperCylinderCollider hyperCylinderCollider = (HyperCylinderCollider)collider;
                result = Math.FindLineCircleIntersections(hyperCylinderCollider.HyperTransform.X, hyperCylinderCollider.HyperTransform.Y, hyperCylinderCollider.Radius, Vector3.ProjectOnPlane(ray.From, new Vector3(0f, 0f, 1f)), Vector3.ProjectOnPlane(ray.To, new Vector3(0f, 0f, 1f)), out interSection1, out interSection2);
            }
            else if (type == typeof(HyperPolygonCollider))
            {
                HyperPolygonCollider hyperPolygonCollider = (HyperPolygonCollider)collider;
                result = Math.FindPolygonLineIntersections(Math.TransformedVertices(hyperPolygonCollider.Vertices, hyperPolygonCollider.HyperTransform.Position, hyperPolygonCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);
            }
            else if (type == typeof(HyperPlaneCollider))
            {
                HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider;

                result = Math.FindPolygonLineIntersections(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);

                Vector3 intersection;

                List<Vector3> vertices3D = new List<Vector3>();
                List<Vector2> vertices2D = new List<Vector2>();
                List<Vector2> verticesRotated = new List<Vector2>();

                for (int i = 0; i < hyperPlaneCollider.Vertices.Length; i++)
                    vertices2D.Add(new Vector2(hyperPlaneCollider.Vertices[i].x, hyperPlaneCollider.Vertices[i].y));

                verticesRotated.AddRange(Math.TransformedVertices(vertices2D.ToArray(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.HyperTransform.Angle));
                for (int i = 0; i < verticesRotated.Count; i++)
                    vertices3D.Add(new Vector3(verticesRotated[i].x, verticesRotated[i].y, hyperPlaneCollider.Vertices[i].z));

                if (Math.FindPolygonLineIntersection3D(ray.From, ray.To, vertices3D.ToArray(), out intersection))
                {
                    result = 3;

                    realPoint = intersection;
                }
            }


            if (result == 1)
                realPoint = Vector3.Lerp(ray.From, ray.To, Vector3.ProjectOnPlane(new Vector3(interSection1.x, interSection1.y, 0f) - ray.From, new Vector3(0f, 0f, 1f)).magnitude / Vector3.ProjectOnPlane(ray.To - ray.From, new Vector3(0f, 0f, 1f)).magnitude);

            if (result == 2)
            {
                if (Math.Distance(interSection1.x, interSection1.y, ray.From.x, ray.From.y) < Math.Distance(interSection2.x, interSection2.y, ray.From.x, ray.From.y))
                    realPoint = Vector3.Lerp(ray.From, ray.To, Vector3.ProjectOnPlane(new Vector3(interSection1.x, interSection1.y, 0f) - ray.From, new Vector3(0f, 0f, 1f)).magnitude / Vector3.ProjectOnPlane(ray.To - ray.From, new Vector3(0f, 0f, 1f)).magnitude);
                else
                    realPoint = Vector3.Lerp(ray.From, ray.To, Vector3.ProjectOnPlane(new Vector3(interSection2.x, interSection2.y, 0f) - ray.From, new Vector3(0f, 0f, 1f)).magnitude / Vector3.ProjectOnPlane(ray.To - ray.From, new Vector3(0f, 0f, 1f)).magnitude);
            }

            if (type == typeof(HyperPlaneCollider) && result != 0 && result != 3 && ray.Direction.x != 0f && ray.Direction.y != 0f)
            {
                HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider;
                if (hyperPlaneCollider.IsSolid)
                {
                    if (hyperPlaneCollider.GetZValue(realPoint.x, realPoint.y) < realPoint.z)
                        result = 0;
                }
            }

            if (result != 0 && realPoint.z >= collider.HyperTransform.Z && realPoint.z <= collider.HyperTransform.Z + collider.Height)
            {
                hyperRaycastHit.collider = collider;
                hyperRaycastHit.distance = (realPoint - ray.From).magnitude;
                hyperRaycastHit.hyperTransform = collider.HyperTransform;
                hyperRaycastHit.point = realPoint;

                return true;
            }

            return false;
        }
    }
}
