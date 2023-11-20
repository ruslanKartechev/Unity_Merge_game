using System.Collections.Generic;
using Game.Hunting;
using Game.Levels;
using UnityEngine;

namespace Common.Levels
{
    [System.Serializable]
    public class EnvironmentLevel
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private List<LevelSettings> _levels;

        public string SceneName => _sceneName;
        public ILevelSettings GetLevel(int index) => _levels[index];
        public int Count => _levels.Count;
        
    }
}