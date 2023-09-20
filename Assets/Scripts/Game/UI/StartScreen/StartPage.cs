using System;
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
        [SerializeField] private SpriteChangeButton _playButton;
        [SerializeField] private SpriteChangeButton _shopButton;
        [SerializeField] private SpriteChangeButton _homeButton;
        [SerializeField] private SpriteChangeButton _collectionButton;
        [Space(5)]
        [SerializeField] private Button _settingsButton;
        private IStartPageListener _listener;
        
        public void InitPage(IStartPageListener listener)
        {
            _listener = listener;
            SubButtons();
            Show();
            _homeButton.SetActive();
        }
        
        private void OpenCollection()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Collection", "w");
            _collectionButton.Scale();
        }

        private void OpenSettings()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Settings", "w");
        }

        private void Play()
        {
            CLog.LogWHeader(nameof(StartPage), "Play", "w");
            _playButton.Scale();
            _listener.OnPlay();
        }

        private void BackHome()
        {
            CLog.LogWHeader(nameof(StartPage), "Home button", "w");
            _homeButton.Scale();
        }
        
        private void OpenShop()
        {
            CLog.LogWHeader(nameof(StartPage), "Shop button", "w");
            _shopButton.Scale();
        }
        
        private void Show()
        {
            UIC.UpdateMoneyAndCrystals();
            _mainCanvas.enabled = true;
        }
        
        private void SubButtons()
        {
            _playButton.Btn.onClick.AddListener(Play);
            _collectionButton.Btn.onClick.AddListener(OpenCollection);
            _settingsButton.onClick.AddListener(OpenSettings);
            _homeButton.Btn.onClick.AddListener(BackHome);
            _shopButton.Btn.onClick.AddListener(OpenShop);
        }

  
    }
}