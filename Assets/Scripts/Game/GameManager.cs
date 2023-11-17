using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI;
using MadPixelAnalytics;
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
        [SerializeField] private PregamePage _pregamePage;
        [Space(10)]
        [SerializeField] private GameObject _devConsolePrefab;
        [SerializeField] private DynamicResolutionManager _resolutionManager;
        [SerializeField] private AnalyticsManager _analytics;
        
        
        private void Awake()
        {
            if (!GameState.FirstLaunch)
            {
                ShowStartScreen();
                return;
            }
            GameState.SingleLevelMode = false;
            GameState.FirstLaunch = false;
            DontDestroyOnLoad(gameObject);
            InitFramerate();
            InitContainer();
            InitSaves();
            // InitAnalytics();
            
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                Instantiate(_devConsolePrefab, transform);
            
            if (_bootSettings.ShowTerms)
                _pregamePage.ShowWithTermsPanel(ShowCheat);
            else
                ShowCheat();
            
        }

        // private void Start()
        // {
        //     Facebook.Unity.FB.Init();
        // }

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
        
        private void ShowCheat()
        {
            CLog.LogWHeader("GM", $"Show Cheat {_bootSettings.ShowPregameCheat}", "w");
            if(_bootSettings.ShowPregameCheat)
                _pregamePage.ShowCheat(PlayGame);
            else
                PlayGame();
        }

        private void InitAnalytics()
        {
            CLog.LogWHeader("GM", $"Init analytics {_bootSettings.InitAnalytics}", "w");
            if(!_bootSettings.InitAnalytics)
                return;
            try
            {
                _analytics.Init();
            }
            catch (System.Exception ex)
            {
                Debug.Log($"Exception {ex.Message}\n{ex.StackTrace}");
            }   
        }
        
        private void PlayGame()
        {
            CLog.LogWHeader("GM", "Play game", "w");
            if (GC.PlayerData.TutorPlayed_Attack == false && GC.PlayerData.LevelTotal == 0)
            {
                Debug.Log("Tutorial not played. Start game from lvl_0");
                _pregamePage.ShowDarkening();
                GC.PlayerData.LevelIndex = 0;
                GC.LevelManager.LoadCurrent();
                if(_bootSettings.RunResolutionScaler)
                    _resolutionManager.Begin();
            }
            else
            {
                ShowStartScreen();
                if(_bootSettings.RunResolutionScaler)
                    _resolutionManager.Begin();
            }
        }

        private void OnApplicationQuit()
        {
            GC.DataSaver.Save();
        }

        private void ShowStartScreen()
        {
            CLog.LogWHeader("GM", "Show start screen", "w");
            _pregamePage.Hide();
            SceneManager.LoadScene(_startPageName);
        }
      
    }
}
