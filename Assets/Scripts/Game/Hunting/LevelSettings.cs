using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelSettings), fileName = nameof(LevelSettings), order = 0)]
    public class LevelSettings :  ScriptableObject, ILevelSettings
    {
        [SerializeField] private LevelEnvironment _environment;
        [SerializeField] private GameObject _preyPackPrefab;
        [SerializeField] private List<PreySettings> _preySettings;
        [SerializeField] private int _cameraFlyDir = 1;
        [SerializeField] private float _packMoveSpeed;

        public int CameraFlyDir => _cameraFlyDir;
        public GameObject GetLevelPrefab() => _preyPackPrefab;
        public float PackMoveSpeed => _packMoveSpeed;
        public LevelEnvironment Environment => _environment;
        public List<PreySettings> PreySettingsList => _preySettings;
    }
    
    #if UNITY_EDITOR
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
        }
    }
    #endif
    
}