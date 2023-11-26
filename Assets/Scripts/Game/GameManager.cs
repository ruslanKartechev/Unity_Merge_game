#define SDK
using Common.Saving;
using Game.Dev;
using Game.Saving;
#if SDK
using MadPixelAnalytics;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string _startPageName;
        [SerializeField] private BootSettings _bootSettings;
#if SDK
        [SerializeField] private AnalyticsManager _analytics;
#endif      
        [Header("For test")]
        [SerializeField] private TestLoader _testLoader;
        
        private void Awake()
        {
            GameState.SingleLevelMode = false;
            GameState.FirstLaunch = false;
            DontDestroyOnLoad(gameObject);
            InitFramerate();
            InitContainer();
            InitSaves();
            // InitAnalytics();
            PlayOrCheat();

            // var waitTime = 5f;
            // _testLoader.Show();
            // _testLoader.WaitAndCallback(waitTime, () =>
            // {
            //     _testLoader.Hide();
            //     PlayGame();
            //     // _pregamePage.ShowWithTermsPanel(PlayGame);
            // });
            // PlayGame();
        }

        private void Start()
        {
#if SDK
            Facebook.Unity.FB.Init();
#endif
        }

        private void InitFramerate()
        {
            CLog.LogWHeader("GM", "Init frameRate", "w");
            if(_bootSettings.CapFPS)
                Application.targetFrameRate = _bootSettings.FpsCap;
            else 
                Application.targetFrameRate = 500;
        }

        private void InitContainer()
        {
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
        }

        private void InitSaves()
        {
            CLog.LogWHeader("GM", "Init saves", "w");
            if (_bootSettings.ClearAllSaves)
                GC.DataSaver.Clear();
            var dataInit = gameObject.GetComponent<SavedDataInitializer>();
            dataInit.InitSavedData();    
            if (_bootSettings.doPeriodicSave)
            {
                var saver = gameObject.GetComponent<IPeriodicDataSaver>();
                saver.SetInterval(_bootSettings.dataSavePeriod);
                saver.Begin();
            }
        }
        
        private void PlayOrCheat()
        {
            CLog.LogWHeader("GM", $"Show Cheat {_bootSettings.ShowPregameCheat}", "w");
            // cheat screen removed
            PlayGame();
        }

        private void InitAnalytics()
        {
#if SDK
            CLog.LogWHeader("GM", $"Init analytics {_bootSettings.InitAnalytics}", "w");
            if(!_bootSettings.InitAnalytics)
                return;
            try
            {
                _analytics.Init();
                _analytics.SubscribeToAdsManager();
            }
            catch (System.Exception ex)
            {
                Debug.Log($"Exception {ex.Message}\n{ex.StackTrace}");
            }   
#endif
        }
        
        private void PlayGame()
        {
            CLog.LogWHeader("GM", "Play game", "w");
            if (GC.PlayerData.TutorPlayed_Attack == false && GC.PlayerData.LevelTotal == 0)
            {
                Debug.Log("Tutorial not played. Start game from lvl_0");
                GC.PlayerData.LevelIndex = 0;
                GC.LevelManager.LoadCurrent();
            }
            else
            {
                ShowStartScreen();
            }
        }

        private void OnApplicationQuit()
        {
            GC.DataSaver.Save();
        }

        private void ShowStartScreen()
        {
            CLog.LogWHeader("GM", "Show start screen", "w");
            SceneManager.LoadScene(_startPageName);
        }
      
        
        
    }
}
