#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    [CustomEditor(typeof(WorldMapCameraPoint))]
    public class WorldMapCameraPointEditor : Editor
    {
        private const float btn_width = 140;

        public void OnEnable()
        {
            var me = target as WorldMapCameraPoint;
            me.TrueGetCamPoints();   
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as WorldMapCameraPoint;
 
            
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
            
            GUILayout.Space(15);
            
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
            
            if (GUILayout.Button("CopyFrom", GUILayout.Width(btn_width)))
            {
                me.CopyFrom();   
            }

            if (me.doDraw)
            {
                me.Draw();
                me.SetRotToLook();
            }
        }
    }
}
#endif