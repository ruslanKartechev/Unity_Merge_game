using Game.Dev;
using Game.Saving;
using UnityEngine;

namespace Game
{
    [DefaultExecutionOrder(-1)]
    public class TestGM : MonoBehaviour
    {
        [SerializeField] private BootSettings _bootSettings;
        [SerializeField] private GameObject _devConsolePrefab;
        private ISavedDataInitializer _savedDataInitializer; 
        
        private void Awake()
        {
            if (GameState.SingleLevelMode == false)
                return;
            if(_bootSettings.UseDevUI && DevActions.Instance == null)
                Instantiate(_devConsolePrefab);
            
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