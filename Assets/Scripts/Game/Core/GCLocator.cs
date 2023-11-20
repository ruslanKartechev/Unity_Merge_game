using Common;
using Common.Levels;
using Common.Scenes;
using Common.SlowMotion;
using UnityEngine;

namespace Game.Core
{
    public class GCLocator : MonoBehaviour, IGlobalContainerLocator
    {
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private SceneSwitcher _sceneSwitcher;
        [SerializeField] private SlowMotionManager _slowMotionEffect;
        [SerializeField] private GCLocatorSO _soLocator;

        public void InitContainer()
        {
            GC.SceneSwitcher = _sceneSwitcher;
            GC.LevelManager = _levelManager;
            GC.SlowMotion = _slowMotionEffect;
            GC.Input = _input;
            _soLocator.InitContainer();
        }
        
        

    }


    
}