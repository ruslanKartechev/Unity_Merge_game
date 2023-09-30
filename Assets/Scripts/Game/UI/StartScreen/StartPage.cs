using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private BottomButtons _bottomButtons;
        [Space(5)]
        [SerializeField] private Button _settingsButton;
        private IStartPageListener _listener;
        
        public void InitPage(IStartPageListener listener)
        {
            _listener = listener;
            SubButtons();
            Show();
            _bottomButtons.SetMain();
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

        private void MoveToMap()
        {
            SceneManager.LoadScene("Map");
        }
        
        private void Show()
        {
            UIC.UpdateMoneyAndCrystals();
            _mainCanvas.enabled = true;
        }
        
        private void SubButtons()
        {
            _playButton.Btn.onClick.AddListener(Play);
            _settingsButton.onClick.AddListener(OpenSettings);
            _bottomButtons.OnCollection = () => {};
            _bottomButtons.OnMain = () =>{};
            _bottomButtons.OnMap = MoveToMap;
            
        }

  
    }
}