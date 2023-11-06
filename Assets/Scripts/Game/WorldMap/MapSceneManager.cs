using System;
using System.Collections;
using Game.Hunting.UI;
using Game.Merging;
using UnityEngine;
using UnityEngine.UI;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(10)]
    public class MapSceneManager : MonoBehaviour
    {
        [SerializeField] private float _moveAnimationDelay = .5f;
        [SerializeField] private float _jumpDuration = 1;
        [SerializeField] private float _levelLoadDelay = .33f;
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapPlayerPack _playerPack;
        [SerializeField] private WorldMapManager _mapManager;
        [SerializeField] private Button _playButton;
        [SerializeField] private PowerDisplay _powerDisplay;
        private bool _levelLoadBegan;
        
        
        public void Start()
        {
            _playButton.onClick.AddListener(LoadLevel);
            var currentLevel = GC.PlayerData.LevelTotal;
            ShowCaptureAnimation(currentLevel);
            var level = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            var powerUs = MergeHelper.CalculatePowerUs(GC.ActiveGroupSO.Group());
            var powerEnemy = MergeHelper.CalculatePowerEnemy(level);
            _powerDisplay.SetPower(powerUs, powerEnemy);
        }

        public void ShowLevel(int currentLevel)
        {
            _mapManager.ShowLevel(currentLevel);   
        }

        public void ShowCaptureAnimation(int currentLevel)
        {
            _mapManager.AnimateToPlayer(currentLevel, _moveAnimationDelay);
        }

        public void LoadLevel()
        {
            if (_levelLoadBegan)
                return;
            _levelLoadBegan = true;
            StartCoroutine(Delayed(_jumpDuration * _levelLoadDelay, MoveToNextScene));
            _playerPack.JumpToAttack(_jumpDuration);
        }

        private void MoveToNextScene()
        {
            GC.SceneSwitcher.OpenScene("Merge", (s) => {});
        }

        private IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            onEnd.Invoke();
        }

    }
}