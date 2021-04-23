using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public class HyperPhysics
    {
        /// <summary>Return true if given collider collides with this one.</summary>
        public static bool IsCollideWith(HyperBaseCollider collider1, HyperBaseCollider collider2)
        {
            bool collision = false;

            if (Math.IsCollideZ(collider1.HyperTransform.Z, collider1.Height, collider2.HyperTransform.Z, collider2.Height))
            {
                Type type1 = collider1.GetType();
                Type type2 = collider2.GetType();

                if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider HyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperCylinderCollider HyperCylinderCollider2 = (HyperCylinderCollider)collider2;
                    collision = Math.CircleCircle(HyperCylinderCollider1.HyperTransform.X, HyperCylinderCollider1.HyperTransform.Y, HyperCylinderCollider1.Radius, HyperCylinderCollider2.HyperTransform.X, HyperCylinderCollider2.HyperTransform.Y, HyperCylinderCollider2.Radius);
                }
                else if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperCylinderCollider HyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperPolygonCollider HyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonCircle(Math.TransformedVertices(HyperPolygonCollider2.Vertices, HyperPolygonCollider2.HyperTransform.Position, HyperPolygonCollider2.HyperTransform.Angle), HyperCylinderCollider1.HyperTransform.X, HyperCylinderCollider1.HyperTransform.Y, HyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider HyperCylinderCollider1 = (HyperCylinderCollider)collider2;
                    HyperPolygonCollider HyperPolygonCollider2 = (HyperPolygonCollider)collider1;
                    collision = Math.PolygonCircle(Math.TransformedVertices(HyperPolygonCollider2.Vertices, HyperPolygonCollider2.HyperTransform.Position, HyperPolygonCollider2.HyperTransform.Angle), HyperCylinderCollider1.HyperTransform.X, HyperCylinderCollider1.HyperTransform.Y, HyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperPolygonCollider HyperPolygonCollider1 = (HyperPolygonCollider)collider1;
                    HyperPolygonCollider HyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonPolygon(Math.TransformedVertices(HyperPolygonCollider1.Vertices, HyperPolygonCollider1.HyperTransform.Position, HyperPolygonCollider1.HyperTransform.Angle), Math.TransformedVertices(HyperPolygonCollider2.Vertices, HyperPolygonCollider2.HyperTransform.Position, HyperPolygonCollider2.HyperTransform.Angle));
                }
                else if (type1 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider HyperPlaneCollider = (HyperPlaneCollider)collider1;
                    if (Math.PolygonCircle(Math.TransformedVertices(HyperPlaneCollider.GetFlatPolygon(), HyperPlaneCollider.HyperTransform.Position, HyperPlaneCollider.HyperTransform.Angle), collider2.HyperTransform.X, collider2.HyperTransform.Y, Mathf.Epsilon))
                    {
                        collision = collider2.HyperTransform.Z <= HyperPlaneCollider.GetZValue(collider2.HyperTransform.X, collider2.HyperTransform.Y) ? true : false;
                    }
                }
                else if (type2 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider HyperPlaneCollider = (HyperPlaneCollider)collider2;
                    if (Math.PolygonCircle(Math.TransformedVertices(HyperPlaneCollider.GetFlatPolygon(), HyperPlaneCollider.HyperTransform.Position, HyperPlaneCollider.HyperTransform.Angle), collider1.HyperTransform.X, collider1.HyperTransform.Y, Mathf.Epsilon))
                    {
                        collision = collider1.HyperTransform.Z <= HyperPlaneCollider.GetZValue(collider1.HyperTransform.X, collider1.HyperTransform.Y) ? true : false;
                    }
                }
                else if (type1 == typeof(HyperLineCollider))
                {
                    HyperLineCollider HyperLineCollider = (HyperLineCollider)collider1;
                    HyperRaycastHit HyperRaycastHit;

                    if (Raycast(new HyperRay(HyperLineCollider.From, HyperLineCollider.To), collider2, out HyperRaycastHit))
                    {
                        collision = true;
                    }
                }
                else if (type2 == typeof(HyperLineCollider))
                {
                    HyperLineCollider HyperLineCollider = (HyperLineCollider)collider2;
                    HyperRaycastHit HyperRaycastHit;

                    if (Raycast(new HyperRay(HyperLineCollider.From, HyperLineCollider.To), collider1, out HyperRaycastHit))
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }

        /// <summary>Return true if given collider collides with given ray.</summary>
        public static bool Raycast(HyperRay ray, HyperBaseCollider collider, out HyperRaycastHit HyperRaycastHit)
        {
            HyperRaycastHit = new HyperRaycastHit(null, -1f, Vector3.zero, null);

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
                HyperCylinderCollider HyperCylinderCollider = (HyperCylinderCollider)collider;
                result = Math.FindLineCircleIntersections(HyperCylinderCollider.HyperTransform.X, HyperCylinderCollider.HyperTransform.Y, HyperCylinderCollider.Radius, Vector3.ProjectOnPlane(ray.From, new Vector3(0f, 0f, 1f)), Vector3.ProjectOnPlane(ray.To, new Vector3(0f, 0f, 1f)), out interSection1, out interSection2);
            }
            else if (type == typeof(HyperPolygonCollider))
            {
                HyperPolygonCollider HyperPolygonCollider = (HyperPolygonCollider)collider;
                result = Math.FindPolygonLineIntersections(Math.TransformedVertices(HyperPolygonCollider.Vertices, HyperPolygonCollider.HyperTransform.Position, HyperPolygonCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);
            }
            else if (type == typeof(HyperPlaneCollider))
            {
                HyperPlaneCollider HyperPlaneCollider = (HyperPlaneCollider)collider;

                result = Math.FindPolygonLineIntersections(Math.TransformedVertices(HyperPlaneCollider.GetFlatPolygon(), HyperPlaneCollider.HyperTransform.Position, HyperPlaneCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);

                Vector3 intersection;

                List<Vector3> vertices3D = new List<Vector3>();
                List<Vector2> vertices2D = new List<Vector2>();
                List<Vector2> verticesRotated = new List<Vector2>();

                for (int i = 0; i < HyperPlaneCollider.Vertices.Length; i++)
                    vertices2D.Add(new Vector2(HyperPlaneCollider.Vertices[i].x, HyperPlaneCollider.Vertices[i].y));

                verticesRotated.AddRange(Math.TransformedVertices(vertices2D.ToArray(), HyperPlaneCollider.HyperTransform.Position, HyperPlaneCollider.HyperTransform.Angle));
                for (int i = 0; i < verticesRotated.Count; i++)
                    //vertices3D.Add(Math.SpaceToWorld(new Vector3(verticesRotated[i].x, verticesRotated[i].y, HyperPlaneCollider.Vertices[i].z)));
                    vertices3D.Add(new Vector3(verticesRotated[i].x, verticesRotated[i].y, HyperPlaneCollider.Vertices[i].z));

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
                HyperPlaneCollider HyperPlaneCollider = (HyperPlaneCollider)collider;
                if (HyperPlaneCollider.IsSolid)
                {
                    if (HyperPlaneCollider.GetZValue(realPoint.x, realPoint.y) < realPoint.z)
                        result = 0;
                }
            }

            if (result != 0 && realPoint.z >= collider.HyperTransform.Z && realPoint.z <= collider.HyperTransform.Z + collider.Height)
            {
                // Blocked By Collided Object
                HyperRaycastHit.collider = collider;
                HyperRaycastHit.distance = (realPoint - ray.From).magnitude;
                HyperRaycastHit.hyperTransform = collider.HyperTransform;
                HyperRaycastHit.point = realPoint;

                return true;
            }

            return false;
        }
    }
}
