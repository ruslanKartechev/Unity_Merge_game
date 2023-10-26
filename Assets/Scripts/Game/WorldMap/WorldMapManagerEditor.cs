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

            if (EU.ButtonBig("Get All", Color.green))
            {
                me.GetParts();
            }
            
            if (EU.ButtonBig("Show all", Color.green))
            {
                me.ShowAll();
            }
    
            if (EU.ButtonBig("Hide all", Color.green))
            {
                me.HideAll();
            }

        }
    }
}
#endif