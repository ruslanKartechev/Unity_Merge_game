using System.Collections.Generic;
using Game;
using Game.Hunting;
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
            var env = GetLevel();
            Load(env.Environment.SceneName);
        }

        public void LoadPrev()
        {
            var data = GC.PlayerData;
            data.LevelTotal--;
            if (data.LevelTotal < 0)
                data.LevelTotal = 0;
            var env = GetLevel();
            Load(env.Environment.SceneName);   
        }
        
        private ILevelSettings GetLevel()
        {
            var level = _levelsRepository.GetLevel( GC.PlayerData.LevelTotal);
            return level;
        }

        private void Load(string sceneName)
        {
            GC.SceneSwitcher.OpenScene(sceneName, OnLoaded);   
        }
        
        private void OnLoaded(bool success)
        { }
        
    }
}