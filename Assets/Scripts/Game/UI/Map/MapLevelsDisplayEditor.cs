#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.UI.Map
{
    [CustomEditor(typeof(MapLevelsDisplay))]
    public class MapLevelsDisplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            var me = target as MapLevelsDisplay;
            if (GUILayout.Button($"Spawn {me.DebugSpawnLevel}", GUILayout.Width(150)))
            {
                me.ShowLevel(me.DebugSpawnLevel);
            }

            if (GUILayout.Button($"Clear", GUILayout.Width(150)))
            {
                me.ClearSpawned();
            }            
        }
    }
}
#endif