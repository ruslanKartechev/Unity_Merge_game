﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(10)]
    public class MapSceneManager : MonoBehaviour
    {
        [SerializeField] private float _moveAnimationDelay = .5f;
        [SerializeField] private float _jumpDuration = 1;
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapPlayerPack _playerPack;
        [SerializeField] private WorldMapManager _mapManager;
        [SerializeField] private Button _playButton;
        
        private bool _played;
        
        public void Start()
        {
            _playButton.onClick.AddListener(Play);
            var currentLevel = GC.PlayerData.LevelTotal;
            ShowCaptureAnimation(currentLevel - 1);
        }

        public void ShowLevel(int currentLevel)
        {
            _mapManager.ShowLevel(currentLevel);   
        }

        public void ShowCaptureAnimation(int currentLevel)
        {
            _mapManager.AnimateToPlayer(currentLevel, _moveAnimationDelay);
        }

        public void Play()
        {
            if (_played)
                return;
            _played = true;
            _playerPack.Jump(_jumpDuration);
            StartCoroutine(Delayed(_jumpDuration * .5f, MoveToNextLevel));
        }

        private void MoveToNextLevel()
        {
            GC.LevelManager.LoadCurrent();   
        }

        private IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            onEnd.Invoke();
        }

    }
}