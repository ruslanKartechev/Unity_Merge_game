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
        private IPreyPack _preyPack;
        private IHunterPack _hunters;
        
        private float _totalRewardEarned = 0;
        private int _preyKilled;
        private bool _isCompleted;
        
         
        private void Awake()
        {
            _preySpawner = GetComponent<IPreySpawner>();
            _huntPackSpawner = GetComponent<IHuntPackSpawner>();
        }

        public void Init(IHuntUIPage page)
        {
            _uiPage = page;
            _uiPage.SetKillCount(0, _preyKilled);
            SpawnPreyAndHunters();
            _preyPack.OnAllDead += OnAllPreyKilled;
            _preyPack.OnPreyKilled += OnPreyKilled;
            LoadingCurtain.Open(() =>{ });
        }
        
        public void Restart()
        {
            CLog.LogWHeader("HuntManager", "RESTART", "y");
            GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }

        public void Continue()
        {
            CLog.LogWHeader("HuntManager", "Continue", "g");
            GC.PlayerData.LevelIndex++;
            GC.PlayerData.LevelTotal++;
            GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }

        private void OnPreyKilled(IPrey prey)
        {
            _preyKilled++;
            var reward = prey.GetReward();
            _totalRewardEarned += reward;
            GC.PlayerData.Money += reward;
            _uiPage.SetKillCount(_preyKilled, _preyKilled);
            _uiPage.UpdateMoney();
        }
        
        private void OnAllPreyKilled()
        {
            // win on the first kill
            CLog.LogWHeader($"HuntManager", "On all prey killed", "g", "w");
            Win();
        }

        private void SpawnPreyAndHunters()
        {
            var preyPack = _preySpawner.Spawn(_splineComputer, 
                GC.LevelRepository.GetLevelSettings(GC.PlayerData.LevelTotal));
            _hunters = _huntPackSpawner.SpawnPack();
            _hunters.SetPrey(preyPack);
            _preyPack = preyPack;
            preyPack.Activate();
            _hunters.Activate();
            _hunters.OnAllWasted += Loose;
        }

        private void Win()
        {
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            _isCompleted = true;
            _hunters.Win();
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