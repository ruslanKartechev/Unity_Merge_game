using Common.Saving;
using Game.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game.Dev
{
    public class GameManagerNoSdk : MonoBehaviour
    {
        [SerializeField] private string _startPageName;
        [SerializeField] private BootSettings _bootSettings;
        [Header("For test")]
        [SerializeField] private TestLoader _testLoader;
        
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
            // var waitTime = 5f;
            // _testLoader.Show();
            // _testLoader.WaitAndCallback(waitTime, () =>
            // {
            //     _testLoader.Hide();
            //     _pregamePage.ShowWithTermsPanel(PlayGame);
            // });
            PlayGame();
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