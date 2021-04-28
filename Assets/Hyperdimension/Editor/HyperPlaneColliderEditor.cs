using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hyperdimension
{
    [CustomEditor(typeof(HyperPlaneCollider))]
    public class HyperPlaneColliderEditor : Editor
    {
        public void OnSceneGUI()
        {
            var component = target as HyperPlaneCollider;
            
            if (component == null)
                return;

            Vector3 center = Math.SpaceToWorld(component.Position);


            if (component.Vertices == null)
                return;
            if (component.Vertices.Length == 0)
                return;

            List<Vector3> vertices3D = new List<Vector3>();
            List<Vector2> vertices2D = new List<Vector2>();
            List<Vector2> verticesRotated = new List<Vector2>();
            
            for (int i = 0; i < component.Vertices.Length; i++)
                vertices2D.Add(new Vector2(component.Vertices[i].x, component.Vertices[i].y));

            verticesRotated.AddRange(Math.TransformedVertices(vertices2D.ToArray(), component.HyperTransform.Position, component.Offset, component.HyperTransform.Angle));
            for (int i = 0; i < verticesRotated.Count; i++)
                vertices3D.Add(Math.SpaceToWorld(new Vector3(verticesRotated[i].x, verticesRotated[i].y, component.Vertices[i].z + component.Z)));
            vertices3D.Add(Math.SpaceToWorld(new Vector3(verticesRotated[0].x, verticesRotated[0].y, component.Vertices[0].z + component.Z)));

            if (vertices3D.Count == 0)
                return;

            Handles.color = Color.green;
            Handles.DrawAAPolyLine(vertices3D.ToArray());
        }
    }
}