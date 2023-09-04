#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Merging
{
    [CustomEditor(typeof(MergeGridRepository))]
    public class MergeGridRepositoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as MergeGridRepository;
            if (GUILayout.Button($"Debug Current Setup", GUILayout.Width(200)))
                me.DebugSetup();
        }
        
    }
}
#endif