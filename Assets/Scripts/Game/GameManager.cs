using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI;
using Game.UI.StartScreen;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour, IStartPageListener
    {
        [SerializeField] private bool _capFPS;
        [SerializeField] private int _fpsCap = 60;
        [SerializeField] private PregamePage _pregamePage;
        [SerializeField] private BootSettings _bootSettings;
        [SerializeField] private StartPage _startPage;
        [SerializeField] private GameObject _devConsolePrefab;
        [SerializeField] private DynamicResolutionManager _resolutionManager;

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
            
            _pregamePage.ShowWithTermsPanel(PlayGame);
        }

        private void InitFramerate()
        {
            if(_capFPS)
                Application.targetFrameRate = _fpsCap;
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
        
        
        private void PlayGame()
        {
            _pregamePage.Hide();
            if (GC.PlayerData.TutorPlayed_Attack == false)
            {
                Debug.Log("Tutorial not played. Start game from lvl_0");
                GC.PlayerData.LevelIndex = 0;
                GC.PlayerData.LevelTotal = 0;
                GC.LevelManager.LoadCurrent();
                _resolutionManager.Begin();
            }
            else
            {
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
