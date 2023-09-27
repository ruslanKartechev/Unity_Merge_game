using Game;
using Game.Hunting;
using UnityEngine;

namespace Common.Levels
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [SerializeField] private LevelsRepository _levelsRepository;
        
        
        public void LoadCurrent()
        {
            var level = GetLevel();
            Load(level.Environment.SceneName);
        }

        private ILevelSettings GetLevel()
        {
            var count = _levelsRepository.Count;
            var ind = GC.PlayerData.LevelIndex;
            if (ind >= count)
            {
                ind = count - 1;
                GC.PlayerData.LevelIndex = ind;
            }
            var level = _levelsRepository.GetLevel(ind);
            return level;
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