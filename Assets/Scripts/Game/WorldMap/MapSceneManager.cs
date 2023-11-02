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
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapPlayerPack _playerPack;
        [SerializeField] private WorldMapManager _mapManager;
        [SerializeField] private Button _playButton;
        [SerializeField] private PowerDisplay _powerDisplay;
        private bool _played;
        
        
        public void Start()
        {
            _playButton.onClick.AddListener(Play);
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

        public void Play()
        {
            if (_played)
                return;
            _played = true;
            StartCoroutine(Delayed(_jumpDuration * .35f, MoveToNextScene));
            _playerPack.Jump(_jumpDuration);
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