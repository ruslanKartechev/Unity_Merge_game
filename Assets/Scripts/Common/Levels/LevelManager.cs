using Game;
using UnityEngine;

namespace Common.Levels
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [SerializeField] private LevelsRepository _levelsRepository;

        public void LoadCurrent()
        {
            var sceneName = _levelsRepository.GetLevelSceneName(GetCorrectIndex());
            Load(sceneName);
        }

        public void LoadNext()
        {
            var data = GC.PlayerData;
            data.LevelTotal++;
            data.LevelIndex++;
            var index = data.LevelIndex;
            if (data.LevelTotal >= _levelsRepository.TotalCount())
            {
                index = GetRandomIndex(index, _levelsRepository.TotalCount());
            }
            Load(_levelsRepository.GetLevelSceneName(index));
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
            index = Mathf.Clamp(index, 0, _levelsRepository.TotalCount());
            return index;
        } 
    }
}