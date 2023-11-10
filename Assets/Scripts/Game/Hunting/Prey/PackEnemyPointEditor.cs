#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Hunting
{
    [CustomEditor(typeof(PackEnemyPoint))]
    public class PackEnemyPointEditor : Editor
    {
        public void OnEnable()
        {
            var me = target as PackEnemyPoint;
            me.FetchRefs();            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as PackEnemyPoint;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<<", GUILayout.Width(50)))
            {
                me.SpawnPrev();
            }
            if (GUILayout.Button(">>", GUILayout.Width(50)))
            {
                me.SpawnNext();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn", GUILayout.Width(80)))
            {
                me.Spawn();
            }
            if (GUILayout.Button("Clear", GUILayout.Width(80)))
            {
                me.Clear();
            }
            me.Move();
            GUILayout.EndHorizontal();
        }
    }
}
#endif