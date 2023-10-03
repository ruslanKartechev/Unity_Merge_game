using System;
using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI.StartScreen;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour, IStartPageListener
    {
        [SerializeField] private BootSettings _bootSettings;
        [SerializeField] private StartPage _startPage;
        [SerializeField] private LoadingCurtain _curtain;
        [SerializeField] private GameObject _devConsolePrefab;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            DebugSettings.SingleLevelMode = false;
            DontDestroyOnLoad(gameObject);
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                Instantiate(_devConsolePrefab);
            
            if (_bootSettings.doPeriodicSave)
            {
                var saver = gameObject.GetComponent<IPeriodicDataSaver>();
                saver.SetInterval(_bootSettings.dataSavePeriod);
                saver.Begin();
            }
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
            if (_bootSettings.ClearAllSaves)
                GC.DataSaver.Clear();
            var dataInit = gameObject.GetComponent<SavedDataInitializer>();
            dataInit.InitSavedData();
            _curtain.Init();
            

        }

        private void Start()
        {
            if (GC.PlayerData.TutorPlayed_Attack == false)
            {
                Debug.Log("Tutorial not played. Start game from lvl_0");
                GC.PlayerData.LevelIndex = 0;
                GC.PlayerData.LevelTotal = 0;
                GC.LevelManager.LoadCurrent();
            }
            else
            {
                if (GC.PlayerData.LevelIndex == 0)
                    GC.PlayerData.LevelIndex = 1;
            }
            _startPage.InitPage(this);
        }

        private void OnApplicationQuit()
        {
            GC.DataSaver.Save();
        }

        public void OnPlay()
        {
        }
    }
    
    
}
