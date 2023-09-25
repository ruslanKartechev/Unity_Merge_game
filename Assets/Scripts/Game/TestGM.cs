using Game.Saving;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-1)]
    public class TestGM : MonoBehaviour
    {
        [SerializeField] private BootSettings _bootSettings;
        private ISavedDataInitializer _savedDataInitializer; 
        
        private void Awake()
        {
            if (DebugSettings.SingleLevelMode == false)
                return;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            
            if(_bootSettings.UseDebugConsole)
                SRDebug.Init();
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
            if (_bootSettings.ClearAllSaves)
                GC.DataSaver.Clear();
            
            _savedDataInitializer = gameObject.GetComponent<ISavedDataInitializer>();
            _savedDataInitializer?.InitSavedData();
        }

    }
}