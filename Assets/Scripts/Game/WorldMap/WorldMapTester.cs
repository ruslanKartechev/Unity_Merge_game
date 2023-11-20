using System.Collections;
using Game.Core;
using Game.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(0)]
    public class WorldMapTester : MonoBehaviour
    {
        [SerializeField] private bool _doWork;
        [SerializeField] private GCLocator _locator;
        [SerializeField] private SavedDataInitializer _dataInitializer;
        
        private void Start()
        {
            if (_doWork && GameState.SingleLevelMode)
            {
                _locator.InitContainer();
                _dataInitializer.InitSavedData();
                StartCoroutine(InputTaking());
            }
        }

        private IEnumerator InputTaking()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    yield break;
                }
                yield return null;
            }
        }

        public void LoadCurrent()
        {
        }

        public void LoadNext()
        {
        }

        public void LoadPrev()
        {
        }
    }
    

}