#if UNITY_EDITOR
using Game.Levels;
using UnityEditor;
using UnityEngine;

namespace Game.Hunting
{
    [CustomEditor(typeof(LevelSettings))]
    public class LevelSettingsEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as LevelSettings;
            var totalHealth = 0f;
            var reward = 0f;
            foreach (var settings in me.PreySettingsList)
            {
                totalHealth += settings.Health;
                reward += settings.Reward;
            }
            GUILayout.Space(30);
            GUILayout.Label($"Total Health: {totalHealth}");
            GUILayout.Label($"Reward: {reward}");
            if (GUILayout.Button("Get In Folder", GUILayout.Width(200)))
            {
                me.GetInTheFolder();
            }
        }
    }
}
#endif