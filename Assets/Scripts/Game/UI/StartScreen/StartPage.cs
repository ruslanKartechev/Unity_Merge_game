using System;
using System.Collections.Generic;
using Game.UI.Map;
using Game.UI.Merging;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI.StartScreen
{
    [DefaultExecutionOrder(5)]
    public class StartPage : MonoBehaviour
    {
        [SerializeField] private string _mergeSceneName = "Merge";
        [Space(10)]
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private List<GameObject> _hideTargets;
        [SerializeField] private SuperEggsPopupTimer _eggsPopupTimer;
        [SerializeField] private LevelsMap _map;
        [SerializeField] private GameObject _mapGO;
        [Space(10)]
        [SerializeField] private SpriteChangeButton _playButton;
        [SerializeField] private BottomButtons _bottomButtons;
        [SerializeField] private Button _settingsButton;
        private IStartPageListener _listener;
        private bool _isMainPage;
        
        public void InitPage(IStartPageListener listener)
        {
            gameObject.SetActive(true);
            _listener = listener;
            SubButtons();
            MainPage();
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
            if (_isMainPage)
                MoveToMap();
            else
                GC.SceneSwitcher.OpenScene(_mergeSceneName, OnLoaded);
        }
        
        private void OnLoaded(bool success)
        {
            if(!success)
                Debug.LogError($"Merging scene was not loaded");
        }

        private void MoveToMap()
        {
            _bottomButtons.SetMap();
            _eggsPopupTimer.Hide();
            ShowScene(false);
            _mapGO.SetActive(true);
            _map.ShowCurrentLevel();
            _isMainPage = false;
        }
        
        private void MainPage()
        {
            _bottomButtons.SetMain();
            UIC.UpdateMoneyAndCrystals();
            _mainCanvas.enabled = true;
            _mapGO.SetActive(false);
            _eggsPopupTimer.Show();
            _isMainPage = true;
            ShowScene(true);
        }

        private void ShowScene(bool active)
        {
            foreach (var go in _hideTargets)
                go.SetActive(active);
        }
        
        private void SubButtons()
        {
            _playButton.Btn.onClick.AddListener(Play);
            _settingsButton.onClick.AddListener(OpenSettings);
            _bottomButtons.OnCollection = () => {};
            _bottomButtons.OnMain = MainPage;
            _bottomButtons.OnMap = MoveToMap;
        }
  
    }
}