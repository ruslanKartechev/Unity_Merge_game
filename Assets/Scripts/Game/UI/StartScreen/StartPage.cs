using Common.UIEffects;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI.StartScreen
{
    [DefaultExecutionOrder(10)]
    public class StartPage : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [Space(10)]
        [SerializeField] private Button _playButton;
        [SerializeField] private ScaleEffect _playSE;
        [Space(5)]
        [SerializeField] private Button _shopButton;
        [SerializeField] private ScaleEffect _shopSE;
        [Space(5)]
        [SerializeField] private Button _homeButton;
        [SerializeField] private ScaleEffect _homeSE;
        [Space(5)]
        [SerializeField] private Button _collectionButton;
        [SerializeField] private ScaleEffect _collectionSE;
        [Space(5)]
        [SerializeField] private Button _settingsButton;
        private IStartPageListener _listener;
        
        public void InitPage(IStartPageListener listener)
        {
            _listener = listener;
            SubButtons();
            Show();
        }
        
        private void OpenCollection()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Collection", "w");
            _collectionSE.Play();
        }

        private void OpenSettings()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Settings", "w");
        }

        private void Play()
        {
            CLog.LogWHeader(nameof(StartPage), "Play", "w");
            _playSE.Play();
            _listener.OnPlay();
        }

        private void BackHome()
        {
            CLog.LogWHeader(nameof(StartPage), "Home button", "w");
            _homeSE.Play();
        }
        
        private void OpenShop()
        {
            CLog.LogWHeader(nameof(StartPage), "Shop button", "w");
            _shopSE.Play();
        }
        
        private void Show()
        {
            UIC.UpdateMoneyAndCrystals();
            _mainCanvas.enabled = true;
        }
        
        private void SubButtons()
        {
            _playButton.onClick.AddListener(Play);
            _collectionButton.onClick.AddListener(OpenCollection);
            _settingsButton.onClick.AddListener(OpenSettings);
            _homeButton.onClick.AddListener(BackHome);
            _shopButton.onClick.AddListener(OpenShop);
        }

  
    }
}