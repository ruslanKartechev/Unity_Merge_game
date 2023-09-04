using Common;
using Dreamteck.Splines;
using Game.Hunting.UI;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class HuntingManager : MonoBehaviour
    {
        
        [SerializeField] private SplineComputer _splineComputer;
        private IPreySpawner _preySpawner;
        private IHuntPackSpawner _huntPackSpawner;
        private IHuntUIPage _uiPage;
        private IPrey _currentPrey;
        private IHunterPack _pack;
        
        private float _totalRewardEarned = 0;
        private int _preyKilled;
        private bool _isCompleted;
        private int PreyCount => 1; // work with 1 only prey so far
         
        private void Awake()
        {
            _preySpawner = GetComponent<IPreySpawner>();
            _huntPackSpawner = GetComponent<IHuntPackSpawner>();
        }

        public void Init(IHuntUIPage page)
        {
            _uiPage = page;
            _uiPage.SetKillCount(0, PreyCount);
            SpawnPreyAndHunters();
            _currentPrey.OnKilled += OnPreyKilled;
            LoadingCurtain.Open(() =>
            { });
        }
        
        public void Restart()
        {
            CLog.LogWHeader("HuntManager", "RESTART", "y");
            Container.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }

        public void Continue()
        {
            CLog.LogWHeader("HuntManager", "Continue", "g");
            Container.PlayerData.LevelIndex++;
            Container.PlayerData.LevelTotal++;
            Container.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }
        
        private void OnPreyKilled(IPrey prey)
        {
            var reward =prey.GetReward();
            _totalRewardEarned += reward;
            Container.PlayerData.Money += reward;
            _uiPage.SetKillCount(1, PreyCount);
            _uiPage.UpdateMoney();
            // win on the first kill
            Win();
        }

        private void SpawnPreyAndHunters()
        {
            var prey = _preySpawner.Spawn(_splineComputer, 
                Container.LevelRepository.GetLevelSettings(Container.PlayerData.LevelTotal));
            _pack = _huntPackSpawner.SpawnPack();
            _pack.SetPrey(prey);
            _currentPrey = prey;
            prey.Activate();
            _pack.Activate();
            _pack.OnAllWasted += Loose;
        }

        private void Win()
        {
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            _isCompleted = true;
            _pack.Win();
            _uiPage.Win(_totalRewardEarned);
        }
        
        private void Loose()
        {
            if (_isCompleted)
                return;
            _isCompleted = true;
            CLog.LogWHeader("HuntManager", "Hunt lost", "w");
            _uiPage.Fail();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Win();
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                Loose();
            }
        }
#endif
    }
}