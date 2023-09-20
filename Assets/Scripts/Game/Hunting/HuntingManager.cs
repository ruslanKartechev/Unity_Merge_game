using Common;
using Dreamteck.Splines;
using Game.Hunting.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game.Hunting
{
    [DefaultExecutionOrder(10)]
    public class HuntingManager : MonoBehaviour
    {
        [SerializeField] private bool _openCurtains = true;
        [SerializeField] private bool _reloadScene = false;
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private IdleEnvironmentConcealer _environmentConcealer;
        
        private IPreySpawner _preySpawner;
        private IHuntPackSpawner _huntPackSpawner;
        private IHuntUIPage _uiPage;
        private IPreyPack _preyPack;
        private IHunterPack _hunters;
        
        private float _totalRewardEarned = 0;
        private int _preyKilled;
        private bool _isCompleted;
        private int _totalPrey;
         
        private void Awake()
        {
            _preySpawner = GetComponent<IPreySpawner>();
            _huntPackSpawner = GetComponent<IHuntPackSpawner>();
        }

        public void Init(IHuntUIPage page)
        {
            _uiPage = page;
            SpawnPreyAndHunters();
            _preyPack.OnAllDead += OnAllPreyKilled;
            _preyPack.OnPreyKilled += OnPreyKilled;
            _totalPrey = _preyPack.PreyCount;
            _uiPage.SetKillCount(0, _totalPrey);
            if(DebugSettings.SingleLevelMode)
                LoadingCurtain.Open(() =>{ });
        }
        
        public void Restart()
        {
            CLog.LogWHeader("HuntManager", "RESTART", "y");
            if(DebugSettings.SingleLevelMode)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }

        public void Continue()
        {
            CLog.LogWHeader("HuntManager", "Continue clicked", "g");
            if (DebugSettings.SingleLevelMode)
            {
                Restart();
                return;
            }
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
            _uiPage.SetKillCount(_preyKilled, _totalPrey);
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
            preyPack.Idle();
            _hunters.IdleState();
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
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Break();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
#endif
    }
}