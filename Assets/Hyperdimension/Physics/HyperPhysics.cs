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
            
            if (Math.IsCollideZ(collider1.Z, collider1.Height, collider2.Z, collider2.Height))// || (type1 == typeof(HyperLineCollider) || type2 == typeof(HyperLineCollider)))
            {
                Type type1 = collider1.GetType();
                Type type2 = collider2.GetType();
                
                if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperCylinderCollider hyperCylinderCollider2 = (HyperCylinderCollider)collider2;
                    collision = Math.CircleCircle(hyperCylinderCollider1.X, hyperCylinderCollider1.Y, hyperCylinderCollider1.Radius, hyperCylinderCollider2.X, hyperCylinderCollider2.Y, hyperCylinderCollider2.Radius);
                }
                else if (type1 == typeof(HyperCylinderCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider1;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.Offset, hyperPolygonCollider2.HyperTransform.Angle), hyperCylinderCollider1.X, hyperCylinderCollider1.Y, hyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperCylinderCollider))
                {
                    HyperCylinderCollider hyperCylinderCollider1 = (HyperCylinderCollider)collider2;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider1;
                    collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.Offset, hyperPolygonCollider2.HyperTransform.Angle), hyperCylinderCollider1.X, hyperCylinderCollider1.Y, hyperCylinderCollider1.Radius);
                }
                else if (type1 == typeof(HyperPolygonCollider) && type2 == typeof(HyperPolygonCollider))
                {
                    HyperPolygonCollider hyperPolygonCollider1 = (HyperPolygonCollider)collider1;
                    HyperPolygonCollider hyperPolygonCollider2 = (HyperPolygonCollider)collider2;
                    collision = Math.PolygonPolygon(Math.TransformedVertices(hyperPolygonCollider1.Vertices, hyperPolygonCollider1.HyperTransform.Position, hyperPolygonCollider1.Offset, hyperPolygonCollider1.HyperTransform.Angle), Math.TransformedVertices(hyperPolygonCollider2.Vertices, hyperPolygonCollider2.HyperTransform.Position, hyperPolygonCollider2.Offset, hyperPolygonCollider2.HyperTransform.Angle));
                }
                else if (type1 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider1;
                    if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle), collider2.X, collider2.Y, Settings.atomicSize))
                    {
                        collision = collider2.Z <= hyperPlaneCollider.GetZValue(collider2.X, collider2.Y) ? true : false;
                    }
                }
                else if (type2 == typeof(HyperPlaneCollider))
                {
                    HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider2;
                    if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle), collider1.X, collider1.Y, Settings.atomicSize))
                    {
                        collision = collider1.Z <= hyperPlaneCollider.GetZValue(collider1.X, collider1.Y) ? true : false;
                    }
                }
                /*
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
                */
            }

            return collision;
        }


        public static bool Raycast(HyperRay ray, HyperBaseCollider collider, out HyperRaycastHit hyperRaycastHit)
        {
            hyperRaycastHit = null;

            Type type = collider.GetType();

            Vector2 interSection1 = Vector2.zero;
            Vector2 interSection2 = Vector2.zero;
            Vector3 realPoint = Vector3.zero;
            int result = 0;

            bool zRay = ray.Direction.x == 0f && ray.Direction.y == 0f;

            /*
            if (type == typeof(HyperLineCollider))
            {
                return false;
            }
            */
            if (type == typeof(HyperCylinderCollider))
            {
                HyperCylinderCollider hyperCylinderCollider = (HyperCylinderCollider)collider;
                
                if (!zRay)
                {
                    result = Math.FindLineCircleIntersections(hyperCylinderCollider.X, hyperCylinderCollider.Y, hyperCylinderCollider.Radius, Vector3.ProjectOnPlane(ray.From, new Vector3(0f, 0f, 1f)), Vector3.ProjectOnPlane(ray.To, new Vector3(0f, 0f, 1f)), out interSection1, out interSection2);
                }
                else
                {
                    if (Math.CircleCircle(ray.From.x, ray.From.y, Settings.atomicSize, hyperCylinderCollider.X, hyperCylinderCollider.Y, hyperCylinderCollider.Radius))
                        result = 4;
                }
            }
            else if (type == typeof(HyperPolygonCollider))
            {
                HyperPolygonCollider hyperPolygonCollider = (HyperPolygonCollider)collider;
                
                if (!zRay)
                {
                    result = Math.FindPolygonLineIntersections(Math.TransformedVertices(hyperPolygonCollider.Vertices, hyperPolygonCollider.HyperTransform.Position, hyperPolygonCollider.Offset, hyperPolygonCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);
                }
                else
                {
                    if (Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider.Vertices, hyperPolygonCollider.HyperTransform.Position, hyperPolygonCollider.Offset, hyperPolygonCollider.HyperTransform.Angle), ray.From.x, ray.From.y, Settings.atomicSize))
                        result = 4;
                }
            }
            else if (type == typeof(HyperPlaneCollider))
            {
                HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider;

                if (hyperPlaneCollider.IsSolid)
                    result = Math.FindPolygonLineIntersections(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle), ray.From.x, ray.From.y, ray.To.x, ray.To.y, out interSection1, out interSection2);

                Vector3 intersection;

                List<Vector3> vertices3D = new List<Vector3>();
                List<Vector2> vertices2D = new List<Vector2>();
                List<Vector2> verticesRotated = new List<Vector2>();
                
                for (int i = 0; i < hyperPlaneCollider.Vertices.Length; i++)
                    vertices2D.Add(new Vector2(hyperPlaneCollider.Vertices[i].x, hyperPlaneCollider.Vertices[i].y));

                verticesRotated.AddRange(Math.TransformedVertices(vertices2D.ToArray(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle));
                for (int i = 0; i < verticesRotated.Count; i++)
                    vertices3D.Add(new Vector3(verticesRotated[i].x, verticesRotated[i].y, hyperPlaneCollider.Vertices[i].z));
                
                if (zRay && ray.Direction.z > 0f)
                    vertices3D.Reverse();
                //if (Vector3.Dot(Vector3.Cross(vertices3D[0], vertices3D[1]), ray.Direction) > 0f)
                //    vertices3D.Reverse();
                
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

            if (type == typeof(HyperPlaneCollider) && result != 0 && result != 3 && !zRay)
            {
                HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider)collider;
                
                if (hyperPlaneCollider.IsSolid)
                {
                    if (hyperPlaneCollider.GetZValue(realPoint.x, realPoint.y) < realPoint.z)
                        result = 0;
                }
            }

            if (result != 0 && result != 4 && realPoint.z >= collider.Z && realPoint.z <= collider.Z + collider.Height)
            {
                hyperRaycastHit = new HyperRaycastHit(collider, (realPoint - ray.From).magnitude, realPoint, collider.HyperTransform);

                return true;
            }
            if (result == 4)
            {
                float realZ = 0f;
                
                if (Math.ZRayCollide(ray.From.z, ray.To.z, collider.Z, collider.Height, out realZ))
                {
                    realPoint = new Vector3(ray.From.x, ray.From.y, realZ);
                    hyperRaycastHit = new HyperRaycastHit(collider, (realPoint - ray.From).magnitude, realPoint, collider.HyperTransform);

                    return true;
                }
            }

            return false;
        }


        public static bool Raycast(HyperRay ray, out HyperRaycastHit raycastHit, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null)
        {
            raycastHit = null;

            HyperBaseCollider[] colliders = GetValidColliders(ray.Center.x, ray.Center.y, ray.Radius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isActiveAndEnabled == false)
                    continue;
                
                if (colliders[i] == exceptThis)
                    continue;
                
                if (exceptThose != null)
                    if (exceptThose.Contains(colliders[i]))
                        continue;
                
                if (Raycast(ray, colliders[i], out raycastHit))
                    return true;
            }

            return false;
        }


        public static bool RaycastAll(HyperRay ray, out HyperRaycastHit[] raycastHits, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null)
        {
            List<HyperRaycastHit> raycastHitsList = new List<HyperRaycastHit>();
            bool returnValue = false;

            HyperBaseCollider[] colliders = GetValidColliders(ray.Center.x, ray.Center.y, ray.Radius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isActiveAndEnabled == false)
                    continue;
                
                if (colliders[i] == exceptThis)
                    continue;
                
                if (exceptThose != null)
                    if (exceptThose.Contains(colliders[i]))
                        continue;
                
                HyperRaycastHit raycastHit;
                
                if (Raycast(ray, colliders[i], out raycastHit))
                {
                    returnValue = true;
                    raycastHitsList.Add(raycastHit);
                }
            }
            
            raycastHits = raycastHitsList.ToArray();
            
            return returnValue;
        }

        
        public static bool CheckCylinder(float x, float y, float z, float radius, float height, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null)
        {
            HyperBaseCollider[] colliders = GetValidColliders(x, y, radius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isActiveAndEnabled == false)
                    continue;
                
                if (colliders[i] == exceptThis)
                    continue;
                
                if (exceptThose != null)
                    if (exceptThose.Contains(colliders[i]))
                        continue;
                
                bool collision = false;
                
                if (Math.IsCollideZ(z, height, colliders[i].Z, colliders[i].Height))
                {
                    Type type = colliders[i].GetType();

                    if (type == typeof(HyperCylinderCollider))
                    {
                        HyperCylinderCollider hyperCylinderCollider = (HyperCylinderCollider) colliders[i];
                        collision = Math.CircleCircle(x, y, radius, hyperCylinderCollider.X, hyperCylinderCollider.Y, hyperCylinderCollider.Radius);
                    }
                    else if (type == typeof(HyperPolygonCollider))
                    {
                        HyperPolygonCollider hyperPolygonCollider = (HyperPolygonCollider) colliders[i];
                        collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider.Vertices, hyperPolygonCollider.HyperTransform.Position, hyperPolygonCollider.Offset, hyperPolygonCollider.HyperTransform.Angle), x, y, radius);
                    }
                    else if (type == typeof(HyperPlaneCollider))
                    {
                        HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider) colliders[i];
                        if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle), x, y, Settings.atomicSize))
                        {
                            collision = z <= hyperPlaneCollider.GetZValue(x, y) ? true : false;
                        }
                    }
                }

                if (collision) return true;
            }

            return false;
        }
        
        
        public static HyperBaseCollider[] OverlapCylinder(float x, float y, float z, float radius, float height, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null)
        {
            HyperBaseCollider[] colliders = GetValidColliders(x, y, radius);
            List<HyperBaseCollider> returnCollidersList = new List<HyperBaseCollider>();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isActiveAndEnabled == false)
                    continue;
                
                if (colliders[i] == exceptThis)
                    continue;
                
                if (exceptThose != null)
                    if (exceptThose.Contains(colliders[i]))
                        continue;
                
                bool collision = false;
                
                if (Math.IsCollideZ(z, height, colliders[i].Z, colliders[i].Height))
                {
                    Type type = colliders[i].GetType();

                    if (type == typeof(HyperCylinderCollider))
                    {
                        HyperCylinderCollider hyperCylinderCollider = (HyperCylinderCollider) colliders[i];
                        collision = Math.CircleCircle(x, y, radius, hyperCylinderCollider.X, hyperCylinderCollider.Y, hyperCylinderCollider.Radius);
                    }
                    else if (type == typeof(HyperPolygonCollider))
                    {
                        HyperPolygonCollider hyperPolygonCollider = (HyperPolygonCollider) colliders[i];
                        collision = Math.PolygonCircle(Math.TransformedVertices(hyperPolygonCollider.Vertices, hyperPolygonCollider.HyperTransform.Position, hyperPolygonCollider.Offset, hyperPolygonCollider.HyperTransform.Angle), x, y, radius);
                    }
                    else if (type == typeof(HyperPlaneCollider))
                    {
                        HyperPlaneCollider hyperPlaneCollider = (HyperPlaneCollider) colliders[i];
                        if (Math.PolygonCircle(Math.TransformedVertices(hyperPlaneCollider.GetFlatPolygon(), hyperPlaneCollider.HyperTransform.Position, hyperPlaneCollider.Offset, hyperPlaneCollider.HyperTransform.Angle), x, y, Settings.atomicSize))
                        {
                            collision = z <= hyperPlaneCollider.GetZValue(x, y) ? true : false;
                        }
                    }
                }

                if (collision)
                    returnCollidersList.Add(colliders[i]);
            }

            return returnCollidersList.ToArray();
        }
        
        
        //public static bool CheckPolygon(Vector2[] vertices, float height, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null) { }
        //public static bool CheckPlane(Vector3[] vertices, bool isSolid, HyperBaseCollider exceptThis = null, List<HyperBaseCollider> exceptThose = null) { }

        
        public static HyperBaseCollider[] GetValidColliders(float x, float y, float radius)
        {
            HyperBaseCollider[] colliders;
            
            HyperPhysicsManager physicsManager = GameObject.FindObjectOfType<HyperPhysicsManager>();
            HyperZone zone = null;

            if (physicsManager != null)
                zone = physicsManager.Zone;

            if (zone != null)
                colliders = zone.GetValidColliders(x, y, radius);
            else
                colliders = GameObject.FindObjectsOfType<HyperBaseCollider>();

            return colliders;
        }
    }
}
