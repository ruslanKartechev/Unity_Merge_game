using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Game.Merging;
using UnityEngine;

namespace Game
{
    public class GlobalContainerLocator : MonoBehaviour, IGlobalContainerLocator
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private IDataSaver _dataSaver;
        [SerializeField] private SceneSwitcher _sceneSwitcher;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private HuntersRepository huntersRepository;
        [SerializeField] private MergeGridRepository mergeGridRepository;
        [SerializeField] private LevelsRepository _levelsRepository;
        
        public void InitContainer()
        {
            Container.PlayerData = _playerData;
            Container.SceneSwitcher = _sceneSwitcher;
            Container.DataSaver = _dataSaver;
            Container.LevelManager = _levelManager;
            Container.HuntersRepository = huntersRepository;
            Container.MergeGridRepository = mergeGridRepository;
            Container.LevelRepository = _levelsRepository;
        }
    }
}