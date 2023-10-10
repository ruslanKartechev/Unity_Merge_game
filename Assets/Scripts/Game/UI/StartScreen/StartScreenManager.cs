using UnityEngine;

namespace Game.UI.StartScreen
{
    [DefaultExecutionOrder(4)]
    public class StartScreenManager : MonoBehaviour, IStartPageListener
    {
        [SerializeField] private string _mergeSceneName = "Merge";
        [SerializeField] private StartPage _startPage;

        private void Start()
        {
            _startPage.InitPage(this);
        }

        public void OnPlay()
        {
            GC.SceneSwitcher.OpenScene(_mergeSceneName, OnLoaded);
        }
        
        private void OnLoaded(bool success)
        {
            if(!success)
                Debug.LogError($"Merging scene was not loaded");
        }
    }
}