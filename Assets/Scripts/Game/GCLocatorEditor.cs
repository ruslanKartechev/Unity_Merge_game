#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(GCLocator))]
    public class GCLocatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as GCLocator;
            if (GUILayout.Button("Release", GUILayout.Width(100)))
                me.ReleaseMode();
            if (GUILayout.Button("Debug", GUILayout.Width(100)))
                me.DebugMode();
        }
    }
}
#endif