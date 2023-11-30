using System.Collections.Generic;
using Game.Hunting;
using UnityEngine;

namespace Common.Levels
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelsRepository), fileName = nameof(LevelsRepository), order = 0)]
    public class LevelsRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelSettings> _levels;
        [SerializeField] private Vector2Int _randomizeLevelLimits;
        [SerializeField] private List<int> _loopExcludedLevels;
        
        public int Count => _levels.Count;

        public ILevelSettings GetLevel(int index)
        {
            Debug.Log($"[LevelsRepo] supplied: {index}");
            if (index >= _levels.Count)
                index = GetRandomIndex(index-1);
            return _levels[index];   
        }
        
        private int GetRandomIndex(int current)
        {
            var index = UnityEngine.Random.Range(_randomizeLevelLimits.x, _randomizeLevelLimits.y);
            var max_iterations = 50;
            var it_count = 0;
            while ((index == current || _loopExcludedLevels.Contains(index))
                   && it_count < max_iterations)
            {
                index = UnityEngine.Random.Range(_randomizeLevelLimits.x, _randomizeLevelLimits.y);
                it_count++;
            }
            if(it_count >= max_iterations)
                Debug.LogError($"Iterated over {max_iterations} times to get random level index!");
            return index;
        }
        
        // public EnvironmentLevel GetEnvironment(int index)
        // {
        //     if (index >= _environments.Count)
        //         index = 0;
        //     return _environments[index];
        // }
    }
}