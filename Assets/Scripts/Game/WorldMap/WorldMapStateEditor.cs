#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    [CanEditMultipleObjects] [CustomEditor(typeof(WorldMapState))]
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
            
            GUILayout.Space((10));
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Enemy Props", Color.white))
            {
                me.ShowEnemyProps();
                me.HidePlayerProps();
            }
            if (EU.ButtonBig("Player Props", Color.white))
            {
                me.HideEnemyProps();
                me.ShowPlayerProps();
            }
            if (EU.ButtonBig("No props", Color.white))
            {
                me.HideEnemyProps();
                me.HidePlayerProps();
            }
            GUILayout.EndHorizontal();
            
            
            GUILayout.Space((10));
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Fog", Color.white))
            {
                me.ShowFog();
            }
            if (EU.ButtonBig("No Fog", Color.white))
            {
                me.HideFog();
            }
            GUILayout.EndHorizontal();
            
            
            GUILayout.Space((10));
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Coll on", Color.white))
            {
                me.SwitchCollider(true);
            }
            if (EU.ButtonBig("Coll off", Color.white))
            {
                me.SwitchCollider(false);
            }
            GUILayout.EndHorizontal();

        }
    }
}
#endif