using System.Collections.Generic;
using Game.Hunting;
using Game.Levels;
using UnityEngine;

namespace Common.Levels
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelsRepository), fileName = nameof(LevelsRepository), order = 0)]
    public class LevelsRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelSettings> _levels;
        
        public int Count => _levels.Count;

        public ILevelSettings GetLevel(int index)
        {
            if (index >= _levels.Count)
                index = 0;
            return _levels[index];   
        }
        
        // public EnvironmentLevel GetEnvironment(int index)
        // {
        //     if (index >= _environments.Count)
        //         index = 0;
        //     return _environments[index];
        // }
    }
}