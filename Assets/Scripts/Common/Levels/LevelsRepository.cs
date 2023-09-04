using System.Collections.Generic;
using Game.Hunting;
using UnityEngine;

namespace Common.Levels
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelsRepository), fileName = nameof(LevelsRepository), order = 0)]
    public class LevelsRepository : ScriptableObject, ILevelRepository 
    {
        [SerializeField] private List<string> _sceneNames;
        [SerializeField] private List<LevelSettings> _settings;

        public string GetLevelSceneName(int index)
        {
            if (index >= TotalCount())
                return _sceneNames[^1];
            return _sceneNames[index];
        }

        public ILevelSettings GetLevelSettings(int index)
        {
            if (index >= _settings.Count)
                return _settings[^1];
            return _settings[index];
        }


        public int TotalCount() => _sceneNames.Count;

  
    }
}