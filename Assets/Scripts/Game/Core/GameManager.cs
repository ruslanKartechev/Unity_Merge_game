using System.Collections;
using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI;
using MadPixelAnalytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
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
            InitAnalytics();
            
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                Instantiate(_devConsolePrefab, transform);
            
            if (_bootSettings.ShowTerms)
                _pregamePage.ShowWithTermsPanel(ShowCheat);
            else
                ShowCheat();
        }

        private void InitFramerate()
        {
            Debug.Log($"[GM] Init frame rate");
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
            Debug.Log($"[GM] Init saves");
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
            Debug.Log($"[GM] Show Cheat {_bootSettings.ShowPregameCheat}");
            if(_bootSettings.ShowPregameCheat)
                _pregamePage.ShowCheat(PlayGame);
            else
                PlayGame();
        }

        private void InitAnalytics()
        {
            Debug.Log($"[GM] Init analytics {_bootSettings.InitAnalytics}");
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
            Debug.Log($"[GM] Play Game");
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
            Debug.Log("[GM] Show main screen");
            _pregamePage.Hide();
            GameState.FromStartToMap = true;
            SceneManager.LoadScene(SceneNames.MainScene);
        }
        
        // DEBUGGING
        private IEnumerator Working()
        {
            var delay = 2f;
            Debug.Log("Delay");
            yield return new WaitForSeconds(delay);
            Debug.Log("FrameRate, container, saves");
            GameState.SingleLevelMode = false;
            DontDestroyOnLoad(gameObject);
            InitFramerate();
            InitContainer();
            InitSaves();
            yield return new WaitForSeconds(delay);
            Debug.Log("ANAL");
            InitAnalytics();

            yield return new WaitForSeconds(delay);

            Debug.Log("SRD Console");
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            yield return new WaitForSeconds(delay);

            Debug.Log("DEV UI");
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                 Instantiate(_devConsolePrefab, transform);   
            else
                Debug.Log("No devActions instantiated");

            yield return new WaitForSeconds(delay);
            Debug.Log("SHOW TERMS");
            if (_bootSettings.ShowTerms)
                _pregamePage.ShowWithTermsPanel(ShowCheat);
            else
                ShowCheat();
        }
    }
}
