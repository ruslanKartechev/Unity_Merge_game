using System;
using Common;
using Common.Saving;
using Game.Saving;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string _mergeSceneName = "Merge";
        [SerializeField] private BootSettings _bootSettings;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            if (_bootSettings.doPeriodicSave)
            {
                var saver = gameObject.GetComponent<IPeriodicDataSaver>();
                saver.SetInterval(_bootSettings.dataSavePeriod);
                saver.Begin();
            }
        }

        void Start()
        {
            LoadingCurtain.CloseNow();
    
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
            
            if (_bootSettings.ClearAllSaves)
                GC.DataSaver.Clear();
            
            var dataInit = gameObject.GetComponent<SavedDataInitializer>();
            dataInit.InitSavedData();
            GC.SceneSwitcher.OpenScene(_mergeSceneName, OnLoaded);
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
    }
    
    
}
