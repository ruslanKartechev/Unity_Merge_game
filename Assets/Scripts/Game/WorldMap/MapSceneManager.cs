using System;
using System.Collections;
using Common.Utils;
using Game.Core;
using Game.Levels;
using Game.Merging.Interfaces;
using Game.UI.Hunting;
using UnityEngine;
using UnityEngine.UI;
using GC = Game.Core.GC;

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
            SetPower(currentLevel);
            var prevLevel = currentLevel - 1;
            // Current level is the one player will attack
            if (GameState.FromStartToMap || prevLevel == 0)
            {
                CLog.LogWHeader(nameof(MapSceneManager), $"Level to show from game start: {currentLevel}","w");
                GameState.FromStartToMap = false;
                ShowLevel(currentLevel);
            }
            else
            {
                CLog.LogWHeader(nameof(MapSceneManager), $"Level to capture: {currentLevel}","w");
                ShowCaptureAnimation(prevLevel);
            }
        }

        private void SetPower(int levelNum)
        {
            var level = GC.LevelRepository.GetLevel(levelNum);
            var powerUs = MergeHelper.CalculatePowerUs(GC.ActiveGroupSO.Group());
            var powerEnemy = MergeHelper.CalculatePowerEnemy(level);
            _powerDisplay.SetPower(powerUs, powerEnemy);   
        }

        private void ShowLevel(int currentLevel)
        {
            _mapManager.ShowLevel(currentLevel);   
        }

        private void ShowCaptureAnimation(int level)
        {
            AddBonus(level);
            _mapManager.AnimateToPlayer(level, _moveAnimationDelay);
        }

        private void AddBonus(int level)
        {
            var bonus = GC.LevelRepository.GetLevel(level).Bonus;
            if (bonus == null)
                return;
            switch (bonus.Type)
            {
                case LevelBonus.BonusType.Egg:
                    GC.ItemsStash.Stash.AddItem(bonus.Item.Item);
                    break;
            }
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
            if (GameState.SingleLevelMode)
            {
                GC.SceneSwitcher.ReloadCurrent();
                return;
            }
            GC.SceneSwitcher.OpenScene("Merge", (s) => {});
        }

        private IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            onEnd.Invoke();
        }

    }
}