#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    [CustomEditor(typeof(WorldMapCameraPoint))]
    public class WorldMapCameraPointEditor : Editor
    {
        private const float btn_width = 140;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as WorldMapCameraPoint;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Far", GUILayout.Width(btn_width)))
            {
                me.SetCamToFarPoint();
            }
            if (GUILayout.Button("Set Close", GUILayout.Width(btn_width)))
            {
                me.SetCamToClosePoint();   
            }
            if (GUILayout.Button("Set Rotation", GUILayout.Width(btn_width)))
            {
                me.SetRotToLook();   
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Far", GUILayout.Width(btn_width)))
            {
                me.SaveFarPoint();
            }
            if (GUILayout.Button("Save Close", GUILayout.Width(btn_width)))
            {
                me.SaveClosePoint();
            }
            GUILayout.EndHorizontal();
            
            
        }
    }
}
#endif