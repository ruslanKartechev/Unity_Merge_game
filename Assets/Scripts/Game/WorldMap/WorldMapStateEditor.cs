#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    [CustomEditor(typeof(WorldMapState))]
    public class WorldMapStateEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            var me = target as WorldMapState;
            if (EU.ButtonBig("Set Camera", Color.white))
            {
                me.SetCameraToThis();
            }
            if (EU.ButtonBig("Get Offset", Color.white))
            {
                me.CalculateOffsetToCamera();
            }

        }
    }
}
#endif