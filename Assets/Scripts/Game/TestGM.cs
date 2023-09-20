using System;
using Game.Saving;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-1)]
    public class TestGM : MonoBehaviour
    {
        [SerializeField] private BootSettings _bootSettings;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
            if (_bootSettings.ClearAllSaves)
                GC.DataSaver.Clear();
            var dataInit = gameObject.GetComponent<SavedDataInitializer>();
            dataInit?.InitSavedData();
        }

        
        

    }
}