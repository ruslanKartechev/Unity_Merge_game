using Game.Saving;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapTester : MonoBehaviour
    {
        [SerializeField] private bool _doWork;
        [SerializeField] private GCLocator _locator;
        [SerializeField] private SavedDataInitializer _dataInitializer;
        
        private void Start()
        {
            if (_doWork)
            {
                _locator.InitContainer();
                _dataInitializer.InitSavedData();
            }
        }
    }
}