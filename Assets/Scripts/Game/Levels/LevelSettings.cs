﻿using System.Collections.Generic;
using System.IO;
using Game.Hunting;
using Game.Hunting.Prey;
using UnityEditor;
using UnityEngine;

namespace Game.Levels
{
    [CreateAssetMenu(menuName = "SO/Level/" + nameof(LevelSettings), fileName = nameof(LevelSettings), order = 0)]
    public class LevelSettings :  ScriptableObject, ILevelSettings
    {
        [SerializeField] private LevelEnvironment _environment;
        [SerializeField] private GameObject _preyPackPrefab;
        [SerializeField] private List<PreySettings> _preySettings;
        [SerializeField] private int _cameraFlyDir = 1;
        [SerializeField] private LevelBonus _levelBonus;

        public int CameraFlyDir => _cameraFlyDir;
        
        public GameObject GetLevelPrefab() => _preyPackPrefab;
        
        
        public LevelEnvironment Environment => _environment;
        
        public List<PreySettings> PreySettingsList => _preySettings;
        public LevelBonus Bonus => _levelBonus;
        
        
        
        
        #if UNITY_EDITOR
        
        [ContextMenu("GetInFolder")]
        public void GetInTheFolder()
        {
            var num = name.Split('_')[1];
            var path = $"Assets/Config/Levels/Level {num}";
            var files = Directory.GetFiles(path, "*.asset", SearchOption.TopDirectoryOnly);
            _preySettings.Clear();
            var i = 1;
            foreach (var ff in files)
            {
                var so = AssetDatabase.LoadAssetAtPath<PreySettings>(ff);
                if (so == null)
                    continue;
                so.name = $"Prey lvl {num} {i++}";
                AssetDatabase.RenameAsset(ff, so.name);
                _preySettings.Add(so);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        #endif
    }
}