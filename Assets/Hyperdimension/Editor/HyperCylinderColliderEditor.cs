using UnityEngine;
using UnityEditor;

namespace Hyperdimension
{
    [CustomEditor(typeof(HyperCylinderCollider))]
    public class HyperCylinderColliderEditor : Editor
    {
        public void OnSceneGUI()
        {
            var component = target as HyperCylinderCollider;
            
            if (component == null)
                return;
            
            Vector3 center = Math.SpaceToWorld(component.HyperTransform.Position);
            Vector3 normal = new Vector3(0f, Settings.ratioY + Settings.ratioY, Settings.ratioY);
            Vector3 height = Math.SpaceToWorld(Vector3.forward * component.Height);
            Vector3 left = center + Math.SpaceToWorld(Vector3.left * component.Radius);
            Vector3 right = center + Math.SpaceToWorld(Vector3.right * component.Radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(center, normal, component.Radius, 1f);
            Handles.DrawWireDisc(center + height, normal, component.Radius, 1f);
            Handles.DrawLine(left, left + height);
            Handles.DrawLine(right, right + height);
        }
    }
}