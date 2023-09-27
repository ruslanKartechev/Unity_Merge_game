using System;
using Common;
using Common.Saving;
using Game.Dev;
using Game.Saving;
using Game.UI.StartScreen;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(0)]
    public class GameManager : MonoBehaviour, IStartPageListener
    {
        [SerializeField] private string _mergeSceneName = "Merge";
        [SerializeField] private BootSettings _bootSettings;
        [SerializeField] private StartPage _startPage;
        [SerializeField] private LoadingCurtain _curtain;
        [SerializeField] private GameObject _devConsolePrefab;


        private void Awake()
        {
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
            _startPage.InitPage(this);
        }


        private void OnLoaded(bool success)
        {
            if(!success)
                Debug.LogError($"Merging scene was not loaded");
        }

        private void OnApplicationQuit()
        {
            GC.DataSaver.Save();
        }

        public void OnPlay()
        {
            GC.SceneSwitcher.OpenScene(_mergeSceneName, OnLoaded);
        }
    }
    
    
}
