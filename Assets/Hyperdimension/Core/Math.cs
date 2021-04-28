using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperdimension
{
    public class Math
    {
        #region Boundary Check Function

        // Boundary Collision Checker
        public static bool CircleCircle(float c1x, float c1y, float c1r, float c2x, float c2y, float c2r)
        {

            // get distance between the circle's centers
            // use the Pythagorean Theorem to compute the distance
            float distX = c1x - c2x;
            float distY = c1y - c2y;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the sum of the circle's
            // radii, the circles are touching!
            if (distance <= c1r + c2r)
            {
                return true;
            }
            return false;
        }

        public static bool RectRect(float r1x, float r1y, float r1w, float r1h, float r2x, float r2y, float r2w, float r2h)
        {

            // are the sides of one rectangle touching the other?

            if (r1x + r1w >= r2x &&    // r1 right edge past r2 left
                r1x <= r2x + r2w &&    // r1 left edge past r2 right
                r1y + r1h >= r2y &&    // r1 top edge past r2 bottom
                r1y <= r2y + r2h)
            {    // r1 bottom edge past r2 top
                return true;
            }
            return false;
        }

        public static bool CircleRect(float cx, float cy, float radius, float rx, float ry, float rw, float rh)
        {

            // temporary variables to set edges for testing
            float testX = cx;
            float testY = cy;

            // which edge is closest?
            if (cx < rx) testX = rx;      // test left edge
            else if (cx > rx + rw) testX = rx + rw;   // right edge
            if (cy < ry) testY = ry;      // top edge
            else if (cy > ry + rh) testY = ry + rh;   // bottom edge

            // get distance from closest edges
            float distX = cx - testX;
            float distY = cy - testY;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            if (distance <= radius)
            {
                return true;
            }
            return false;
        }

        public static bool LineCircle(float x1, float y1, float x2, float y2, float cx, float cy, float r)
        {

            // is either end INSIDE the circle?
            // if so, return true immediately
            bool inside1 = PointCircle(x1, y1, cx, cy, r);
            bool inside2 = PointCircle(x2, y2, cx, cy, r);
            if (inside1 || inside2) return true;

            // get length of the line
            float distX = x1 - x2;
            float distY = y1 - y2;
            float len = Mathf.Sqrt((distX * distX) + (distY * distY));

            // get dot product of the line and circle
            float dot = (((cx - x1) * (x2 - x1)) + ((cy - y1) * (y2 - y1))) / Mathf.Pow(len, 2);

            // find the closest point on the line
            float closestX = x1 + (dot * (x2 - x1));
            float closestY = y1 + (dot * (y2 - y1));

            // is this point actually on the line segment?
            // if so keep going, but if not, return false
            bool onSegment = LinePoint(x1, y1, x2, y2, closestX, closestY);
            if (!onSegment) return false;

            // get distance to closest point
            distX = closestX - cx;
            distY = closestY - cy;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // is the circle on the line?
            if (distance <= r)
            {
                return true;
            }
            return false;
        }


        // LINE/POINT
        public static bool LinePoint(float x1, float y1, float x2, float y2, float px, float py)
        {

            // get distance from the point to the two ends of the line
            float d1 = Distance(px, py, x1, y1);
            float d2 = Distance(px, py, x2, y2);

            // get the length of the line
            float lineLen = Distance(x1, y1, x2, y2);

            // since floats are so minutely accurate, add
            // a little buffer zone that will give collision
            float buffer = 0.1f;    // higher # = less accurate

            // if the two distances are equal to the line's
            // length, the point is on the line!
            // note we use the buffer here to give a range, rather
            // than one #
            if (d1 + d2 >= lineLen - buffer && d1 + d2 <= lineLen + buffer)
            {
                return true;
            }
            return false;
        }


        // POINT/CIRCLE
        public static bool PointCircle(float px, float py, float cx, float cy, float r)
        {

            // get distance between the point and circle's center
            // using the Pythagorean Theorem
            float distX = px - cx;
            float distY = py - cy;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the circle's 
            // radius the point is inside!
            if (distance <= r)
            {
                return true;
            }
            return false;
        }


        // POLYGON/POINT
        // only needed if you're going to check if the circle
        // is INSIDE the polygon


        public static float Distance(float px, float py, float x1, float y1)
        {
            return Mathf.Sqrt(Mathf.Pow(px - x1, 2) + Mathf.Pow(py - y1, 2));
        }

        public static bool PolygonPoint(Vector2[] vertices, float px, float py)
        {
            bool collision = false;

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                Vector2 vc = vertices[current];    // c for "current"
                Vector2 vn = vertices[next];       // n for "next"

                // compare position, flip 'collision' variable
                // back and forth
                if (((vc.y > py && vn.y < py) || (vc.y < py && vn.y > py)) &&
                     (px < (vn.x - vc.x) * (py - vc.y) / (vn.y - vc.y) + vc.x))
                {
                    collision = !collision;
                }
            }
            return collision;
        }
        public static bool PolygonCircle(Vector2[] vertices, float cx, float cy, float r)
        {

            // go through each of the vertices, plus
            // the next vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                Vector2 vc = vertices[current];    // c for "current"
                Vector2 vn = vertices[next];       // n for "next"

                // check for collision between the circle and
                // a line formed between the two vertices
                bool collision = LineCircle(vc.x, vc.y, vn.x, vn.y, cx, cy, r);
                if (collision) return true;
            }

            // the above algorithm only checks if the circle
            // is touching the edges of the polygon – in most
            // cases this is enough, but you can un-comment the
            // following code to also test if the center of the
            // circle is inside the polygon

            bool centerInside = PolygonPoint(vertices, cx, cy);
            if (centerInside) return true;

            // otherwise, after all that, return false
            return false;
        }

        // POLYGON/POLYGON
        public static bool PolygonPolygon(Vector2[] p1, Vector2[] p2)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < p1.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == p1.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                Vector2 vc = p1[current];    // c for "current"
                Vector2 vn = p1[next];       // n for "next"

                // now we can use these two points (a line) to compare
                // to the other polygon's vertices using polyLine()
                bool collision = PolygonLine(p2, vc.x, vc.y, vn.x, vn.y);
                if (collision) return true;

                // optional: check if the 2nd polygon is INSIDE the first
                collision = PolygonPoint(p1, p2[0].x, p2[0].y);
                if (collision) return true;
            }

            return false;
        }


        // POLYGON/LINE
        public static bool PolygonLine(Vector2[] vertices, float x1, float y1, float x2, float y2)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // extract X/Y coordinates from each
                float x3 = vertices[current].x;
                float y3 = vertices[current].y;
                float x4 = vertices[next].x;
                float y4 = vertices[next].y;

                // do a Line/Line comparison
                // if true, return 'true' immediately and
                // stop testing (faster)
                bool hit = LineLine(x1, y1, x2, y2, x3, y3, x4, y4);
                if (hit)
                {
                    return true;
                }
            }

            // never got a hit
            return false;
        }


        // LINE/LINE
        public static bool LineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {

            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return true;
            }
            return false;
        }

        public static bool FindLineLineIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, out Vector2 intersection)
        {

            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            intersection = new Vector2();

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                float intersectionX = x1 + (uA * (x2 - x1));
                float intersectionY = y1 + (uA * (y2 - y1));



                intersection.x = intersectionX;
                intersection.y = intersectionY;

                return true;
            }
            return false;
        }

        public static int FindLineCircleIntersections(float cx, float cy, float radius, Vector2 point1, Vector2 point2, out Vector2 intersection1, out Vector2 intersection2)
        {
            float dx, dy, A, B, C, det, t;

            dx = point2.x - point1.x;
            dy = point2.y - point1.y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (point1.x - cx) + dy * (point1.y - cy));
            C = (point1.x - cx) * (point1.x - cx) +
                (point1.y - cy) * (point1.y - cy) -
                radius * radius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if (det == 0)
            {
                // One solution.
                t = -B / (2 * A);
                intersection1 =
                    new Vector2(point1.x + t * dx, point1.y + t * dy);
                intersection2 = new Vector2(float.NaN, float.NaN);

                if (!LinePoint(point1.x, point1.y, point2.x, point2.y, intersection1.x, intersection1.y))
                    return 0;

                return 1;
            }
            else
            {
                // Two solutions.
                t = (float)((-B + Mathf.Sqrt(det)) / (2 * A));
                intersection1 =
                    new Vector2(point1.x + t * dx, point1.y + t * dy);
                t = (float)((-B - Mathf.Sqrt(det)) / (2 * A));
                intersection2 =
                    new Vector2(point1.x + t * dx, point1.y + t * dy);

                if (!LinePoint(point1.x, point1.y, point2.x, point2.y, intersection1.x, intersection1.y) && !LinePoint(point1.x, point1.y, point2.x, point2.y, intersection2.x, intersection2.y))
                    return 0;

                return 2;
            }
        }

        // POLYGON/LINE
        public static int FindPolygonLineIntersections(Vector2[] vertices, float x1, float y1, float x2, float y2, out Vector2 intersection1, out Vector2 intersection2)
        {
            intersection1 = new Vector2();
            intersection2 = new Vector2();

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            int count = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // extract X/Y coordinates from each
                float x3 = vertices[current].x;
                float y3 = vertices[current].y;
                float x4 = vertices[next].x;
                float y4 = vertices[next].y;

                // do a Line/Line comparison
                // if true, return 'true' immediately and
                // stop testing (faster)

                Vector2 intersection;

                bool hit = FindLineLineIntersection(x1, y1, x2, y2, x3, y3, x4, y4, out intersection);
                if (hit)
                {
                    if (count == 0)
                        intersection1 = intersection;
                    if (count == 1)
                    {
                        intersection2 = intersection;
                        return 2;
                    }
                    count++;
                }
            }

            return count;
        }

        public static bool IsCollideZ(float obj1Z, float obj1Height, float obj2Z, float obj2Height)
        {

            if (obj1Z <= obj2Z + obj2Height && obj1Z >= obj2Z)
                return true;

            if (obj1Z + obj1Height <= obj2Z + obj2Height && obj1Z + obj1Height >= obj2Z)
                return true;

            if (obj2Z <= obj1Z + obj1Height && obj2Z >= obj1Z)
                return true;

            if (obj2Z + obj2Height <= obj1Z + obj1Height && obj2Z + obj2Height >= obj1Z)
                return true;


            return false;
        }

        public static bool FindTriangleLineIntersection3D(Vector3 rayP1, Vector3 rayP2, Vector3[] polygon, out Vector3 intersection)
        {
            intersection = new Vector3();

            Vector3 e1 = polygon[1] - polygon[0];
            Vector3 e2 = polygon[2] - polygon[0];
            Vector3 n = Vector3.Cross(e1, e2).normalized;
            Vector3 w = (rayP2 - rayP1).normalized;

            Vector3 q = Vector3.Cross(w, e2);
            float a = Vector3.Dot(e1, q);

            if (Vector3.Dot(n, w) >= 0 || Mathf.Abs(a) <= Mathf.Epsilon)
                return false;

            Vector3 s = (rayP1 - polygon[0]) / a;
            Vector3 r = Vector3.Cross(s, e1);

            float b0 = Vector3.Dot(s, q);
            float b1 = Vector3.Dot(r, w);
            float b2 = 1f - b0 - b1;

            if (b0 < 0f || b1 < 0f || b2 < 0f)
                return false;

            float t = Vector3.Dot(e2, r);

            if (t > (rayP2 - rayP1).magnitude)
                return false;

            intersection = rayP1 + (w * t);

            return true;
        }

        public static bool FindPolygonLineIntersection3D(Vector3 rayP1, Vector3 rayP2, Vector3[] polygon, out Vector3 intersection)
        {
            intersection = new Vector3();
            Vector3 intersectionTemp;

            for (int i = 1; i < polygon.Length - 1; i++)
            {
                if (FindTriangleLineIntersection3D(rayP1, rayP2, new Vector3[] { polygon[0], polygon[i], polygon[i + 1] }, out intersectionTemp))
                {
                    intersection = intersectionTemp;
                    return true;
                }
            }

            return false;
        }

        public static bool ZRayCollide(float rayZ1, float rayZ2, float objZ, float objHeight, out float collideZ)
        {
            collideZ = rayZ1;
            
            float rayLow = rayZ1;
            float rayHigh = rayZ2;
                
            if (rayZ1 > rayZ2)
            {
                rayHigh = rayZ1;
                rayLow = rayZ2;
            }

            if (Math.IsCollideZ(rayLow, rayHigh - rayLow, objZ, objHeight))
            {
                if (rayZ1 < rayZ2)
                {
                    if (rayZ1 < objZ)
                        collideZ = objZ;
                    else
                        collideZ = objZ + objHeight;
                }
                if (rayZ1 > rayZ2)
                {
                    if (rayZ1 > objZ + objHeight)
                        collideZ = objZ + objHeight;
                    else
                        collideZ = objZ;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Coordination Function

        public static Vector3 WorldToSpace(Vector3 world)
        {
            //Debug.Log("WTS " + world + " " + new Vector3(world.x / Settings.ratioX, world.y / Settings.ratioY - world.z / Settings.ratioZ, world.z));
            return new Vector3(world.x / Settings.ratioX, world.y / Settings.ratioY - world.z / Settings.ratioZ, world.z);
        }
        public static Vector3 SpaceToWorld(Vector3 space)
        {
            //Debug.Log("STW " + space + " " + new Vector3(space.x * Settings.ratioX, space.y * Settings.ratioY + space.z * Settings.ratioZ, space.z));
            return new Vector3(space.x * Settings.ratioX, space.y * Settings.ratioY + space.z * Settings.ratioZ, space.z);
        }

        public static Vector3 ChangeYZ(Vector3 vector)
        {
            return new Vector3(vector.x, vector.z, vector.y);
        }

        #endregion

        #region Transformation Function

        public static Vector2 RotatedVertex(Vector2 vertex, float angle)
        {
            Matrix4x4 m = Matrix4x4.identity;
            m.SetTRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
            Vector3 v = vertex;
            return m.MultiplyPoint3x4(v);
        }

        public static Vector2[] TransformedVertices(Vector2[] vertices, Vector2 centerPosition, Vector2 offset, float angle)
        {
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < vertices.Length; i++)
            {
                list.Add(RotatedVertex(vertices[i] + offset, angle) + centerPosition);
            }

            return list.ToArray();
        }

        #endregion
    }
}