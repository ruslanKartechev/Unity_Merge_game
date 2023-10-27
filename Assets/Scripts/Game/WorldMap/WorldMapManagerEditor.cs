#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    [CustomEditor(typeof(WorldMapManager))]
    public class WorldMapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as WorldMapManager;
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Get All", Color.cyan))
            {
                me.GetParts();
            }
            if (EU.ButtonBig("Show all", Color.cyan))
            {
                me.ShowAll();
            }
            if (EU.ButtonBig("Hide all", Color.cyan))
            {
                me.HideAll();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("All Player", Color.green))
            {
                me.SetAllAsPlayerTerritory();
            }
            if (EU.ButtonBig("All enemy", Color.red * .8f))
            {
                me.SetAllAsEnemyTerritory();
            }
            GUILayout.EndHorizontal();

        }
    }
}
#endif