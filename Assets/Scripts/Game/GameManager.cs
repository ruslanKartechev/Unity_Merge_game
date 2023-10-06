using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI;
using Game.UI.StartScreen;
using MadPixelAnalytics;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour, IStartPageListener
    {
        [SerializeField] private BootSettings _bootSettings;
        [SerializeField] private PregamePage _pregamePage;
        [SerializeField] private StartPage _startPage;
        [Space(10)]
        [SerializeField] private GameObject _devConsolePrefab;
        [SerializeField] private DynamicResolutionManager _resolutionManager;

        [SerializeField] private AnalyticsManager _analytics;

        public void OnPlay(){}
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            DebugSettings.SingleLevelMode = false;
            InitFramerate();

            InitContainer();
            InitSaves();
            
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                Instantiate(_devConsolePrefab, transform);
            
            _pregamePage.ShowWithTermsPanel(ShowCheat);
        }

        private void InitFramerate()
        {
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
            if(_bootSettings.ShowPregameCheat)
                _pregamePage.ShowCheat(PlayGame);
            else
                PlayGame();
        }

        private void InitAnalytics()
        {
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
            InitAnalytics();
            if (GC.PlayerData.TutorPlayed_Attack == false)
            {
                _pregamePage.ShowDarkening();
                Debug.Log("Tutorial not played. Start game from lvl_0");
                GC.PlayerData.LevelIndex = 0;
                GC.PlayerData.LevelTotal = 0;
                GC.LevelManager.LoadCurrent();
                _resolutionManager.Begin();
            }
            else
            {
                _pregamePage.Hide();
                _startPage.InitPage(this);
                if (GC.PlayerData.LevelIndex == 0)
                    GC.PlayerData.LevelIndex = 1;
                _resolutionManager.Begin();
            }
        }

        private void OnApplicationQuit()
        {
            GC.DataSaver.Save();
        }
    }
}
