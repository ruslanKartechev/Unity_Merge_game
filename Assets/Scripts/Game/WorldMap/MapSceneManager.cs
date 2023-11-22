using System;
using System.Collections;
using Common.Utils;
using Game.Core;
using Game.Levels;
using Game.Merging.Interfaces;
using Game.UI.Hunting;
using Game.UI.Map;
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
        [SerializeField] private WorldMapPlayerPack _playerPack;
        [SerializeField] private WorldMapManager _mapManager;
        [SerializeField] private WorldMapUI _mapUI;
        private bool _levelLoadBegan;
        
        
        public void Start()
        {
            _mapUI.PlayButton.onClick.AddListener(LoadLevel);
            _mapUI.LevelsOnly();
            var currentLevel = GC.PlayerData.LevelTotal;
            SetPower(currentLevel);
            var prevLevel = currentLevel - 1;
            // Current level is the one player will attack
            if (GameState.FromStartToMap)
            {
                CLog.LogWHeader(nameof(MapSceneManager), $"Level to show from game start: {currentLevel}","w");
                GameState.FromStartToMap = false;
                ShowLevel(currentLevel);
            }
            else if (prevLevel == 0)
            {
                CaptureInPlace(currentLevel);
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
            _mapUI.Power.SetPower(powerUs, powerEnemy);   
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

        private void CaptureInPlace(int level)
        {
            _mapManager.ShowCaptureInPlace(level);
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

        private void LoadLevel()
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