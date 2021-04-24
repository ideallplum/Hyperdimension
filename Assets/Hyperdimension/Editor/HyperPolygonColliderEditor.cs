using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hyperdimension
{
    [CustomEditor(typeof(HyperPolygonCollider))]
    public class HyperPolygonColliderEditor : Editor
    {
        public void OnSceneGUI()
        {
            var component = target as HyperPolygonCollider;
            
            if (component == null)
                return;
            
            Vector3 center = Math.SpaceToWorld(component.HyperTransform.Position);
            Vector3 height = Math.SpaceToWorld(Vector3.forward * component.Height);
            Vector3 left = center + Math.SpaceToWorld(Vector3.left * component.GetLeastRadius());
            Vector3 right = center + Math.SpaceToWorld(Vector3.right * component.GetLeastRadius());

            if (component.Vertices == null)
                return;
            if (component.Vertices.Length == 0)
                return;

            List<Vector3> vertices3D = new List<Vector3>();
            List<Vector3> vertices3D2 = new List<Vector3>();
            List<Vector2> verticesRotated = new List<Vector2>();
            verticesRotated.AddRange(Math.TransformedVertices(component.Vertices, component.HyperTransform.Position, component.HyperTransform.Angle));
            for (int i = 0; i < verticesRotated.Count; i++)
                vertices3D.Add(Math.SpaceToWorld(new Vector3(verticesRotated[i].x, verticesRotated[i].y, component.HyperTransform.Z)));
            vertices3D.Add(Math.SpaceToWorld(new Vector3(verticesRotated[0].x, verticesRotated[0].y, component.HyperTransform.Z)));

            for (int i = 0; i < verticesRotated.Count; i++)
                vertices3D2.Add(Math.SpaceToWorld(new Vector3(verticesRotated[i].x, verticesRotated[i].y, component.HyperTransform.Z + component.Height)));
            vertices3D2.Add(Math.SpaceToWorld(new Vector3(verticesRotated[0].x, verticesRotated[0].y, component.HyperTransform.Z + component.Height)));

            if (vertices3D.Count == 0 || vertices3D2.Count == 0)
                return;

            Handles.color = Color.green;
            Handles.DrawAAPolyLine(vertices3D.ToArray());
            Handles.DrawAAPolyLine(vertices3D2.ToArray());
            Handles.DrawLine(left, left + height);
            Handles.DrawLine(right, right + height);
        }
    }
}