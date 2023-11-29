using Game.Core;
using Game.Saving;
using UnityEngine;
using GC = Game.Core.GC;

namespace Creatives
{
    public class CreativeSceneBoot : MonoBehaviour
    {
        [SerializeField] private bool _clearSaves;
        private ISavedDataInitializer _savedDataInitializer; 
        
        private void Awake()
        {
            if (GameState.SingleLevelMode == false)
                return;
            
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            
            var containerLocator = gameObject.GetComponent<IGlobalContainerLocator>();
            containerLocator.InitContainer();
            if (_clearSaves)
                GC.DataSaver.Clear();
            
            _savedDataInitializer = gameObject.GetComponent<ISavedDataInitializer>();
            _savedDataInitializer?.InitSavedData();
        }
        
    }
}