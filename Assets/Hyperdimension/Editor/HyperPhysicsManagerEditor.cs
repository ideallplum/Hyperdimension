using UnityEngine;
using UnityEditor;

namespace Hyperdimension
{
    [CustomEditor(typeof(HyperPhysicsManager))]
    public class HyperPhysicsManagerEditor : Editor
    {
        public void OnSceneGUI()
        {
            var component = target as HyperPhysicsManager;

            if (component == null)
                return;
            
            float xOrigin = component.ZoneCenter.x - (component.ZoneSizeX * component.ZoneCellSize * 0.5f);
            float yOrigin = component.ZoneCenter.y - (component.ZoneSizeY * component.ZoneCellSize * 0.5f);

            Handles.color = Color.white;

            int count = component.ZoneSizeX >= component.ZoneSizeY ? component.ZoneSizeY : component.ZoneSizeX;
            
            for (int i = 0; i <= component.ZoneSizeY; i++)
            {
                Handles.DrawLine(Math.SpaceToWorld(new Vector3(xOrigin, yOrigin + (i * component.ZoneCellSize), 0f)), Math.SpaceToWorld(new Vector3(xOrigin + (component.ZoneSizeX * component.ZoneCellSize), yOrigin + (i * component.ZoneCellSize), 0f)));
            }
            for (int i = 0; i <= component.ZoneSizeX; i++)
            {
                Handles.DrawLine(Math.SpaceToWorld(new Vector3(xOrigin + (i * component.ZoneCellSize), yOrigin, 0f)), Math.SpaceToWorld(new Vector3(xOrigin + (i * component.ZoneCellSize), yOrigin + (component.ZoneSizeY * component.ZoneCellSize), 0f)));
            }
        }
    }
}