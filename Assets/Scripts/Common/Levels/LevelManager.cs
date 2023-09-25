using Game;
using UnityEngine;

namespace Common.Levels
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [SerializeField] private LevelsRepository _levelsRepository;
        
        
        public void LoadCurrent()
        {
            var env = GetCurrentEnvironment();
            Load(env.SceneName);
        }

        private EnvironmentLevel GetCurrentEnvironment()
        {
            var ind = GC.PlayerData.EnvironmentIndex;
            var env = _levelsRepository.GetEnvironment(ind);
            if (GC.PlayerData.LevelIndex >= env.Count)
            {
                GC.PlayerData.LevelIndex = ind = 0;
                GC.PlayerData.EnvironmentIndex++;
                env = _levelsRepository.GetEnvironment(ind);
            }
            return env;
        }

        public void LoadNext()
        {
            var data = GC.PlayerData;
            data.LevelTotal++;
            data.LevelIndex++;
            var env = GetCurrentEnvironment();
            Load(env.SceneName);
        }

        private void Load(string sceneName)
        {
            GC.SceneSwitcher.OpenScene(sceneName, OnLoaded);   
        }
        
        private void OnLoaded(bool success)
        {
            
        }

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