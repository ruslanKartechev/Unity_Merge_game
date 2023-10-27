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
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Set Player", Color.white))
            {
                me.EditorSetPlayer();
            }
            if (EU.ButtonBig("Set Enemy", Color.white))
            {
                me.EditorSetEnemy();
            }
            GUILayout.EndHorizontal();

        }
    }
}
#endif