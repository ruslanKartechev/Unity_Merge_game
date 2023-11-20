using System.Collections.Generic;
using Game;
using Game.Core;
using Game.Hunting;
using Game.Levels;
using UnityEngine;

namespace Common.Levels
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [SerializeField] private LevelsRepository _levelsRepository;
        [SerializeField] private Vector2Int _randomizeLevelLimits;
        [SerializeField] private List<int> _loopExcludedLevels;

        public void LoadCurrent()
        {
            var level = GetLevel();
            Load(level.Environment.SceneName);
        }
        

        public void LoadNext()
        {
            var data = GC.PlayerData;
            data.LevelTotal++;
            data.LevelIndex++;
            var env = GetLevel();
            Load(env.Environment.SceneName);
        }

        public void LoadPrev()
        {
            var data = GC.PlayerData;
            data.LevelTotal--;
            data.LevelIndex--;
            if (data.LevelIndex < 0)
                data.LevelIndex = 0;
            if (data.LevelTotal < 0)
                data.LevelTotal = 0;
            var env = GetLevel();
            Load(env.Environment.SceneName);   
        }
        
        private ILevelSettings GetLevel()
        {
            var count = _levelsRepository.Count;
            var levelsTotal = GC.PlayerData.LevelTotal;
            var levelIndex = GC.PlayerData.LevelIndex;
            if (levelsTotal >= count || levelIndex >= count)
            {
                Debug.Log($"Total levels {levelsTotal} > levelsCount {count}. Randomizing");
                levelIndex = GetRandomIndex(levelIndex);
                GC.PlayerData.LevelIndex = levelIndex;
            }
            var level = _levelsRepository.GetLevel(levelIndex);
            return level;
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

        private void Load(string sceneName)
        {
            GC.SceneSwitcher.OpenScene(sceneName, OnLoaded);   
        }
        
        private void OnLoaded(bool success)
        { }

        private int GetRandomIndex(int current, int total)
        {
            var index = current;
            while (index == current)
            {
                index = UnityEngine.Random.Range(0, total);
            }
            return index;
        }
        
        private int GetCorrectIndex()
        {
            var data = GC.PlayerData;
            var index = data.LevelIndex;
            index = Mathf.Clamp(index, 0, _levelsRepository.Count);
            return index;
        } 
    }
}