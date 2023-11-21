using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Utils;
using Game.Levels;
using Game.UI.Map;
using UnityEditor;
using GC = Game.Core.GC;

namespace Game.WorldMap
{
    public class WorldMapManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _unlockAnimScaleTime = 1f;
        [SerializeField] private float _unlockAnimFadeTime = 1f;
        [SerializeField] private float jumpDuration = .5f;
        [SerializeField] private float _cameraMoveTime = .5f;
        [SerializeField] private float _uiAnimationTime = .5f;
        [SerializeField] private float _bonusDelay = 1f;
        [Space(20)]        
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapPlayerPack _playerPack;
        [SerializeField] private WorldMapUI _mapUI;
        [SerializeField] private List<WorldMapPart> _worldMapParts;
        private int _animatedLevel;

        private WorldMapPart _currentPart;
        
        
        private int CorrectIndex(int level)
        {
            if(level < 0)
                level = 0;
            var subed = false;
            while (level >= _worldMapParts.Count)
            {
                subed = true;
                level -= _worldMapParts.Count;
            }
            if(subed && level == 0)
                level++;
               
            return level;
        }
        
        public void ShowLevel(int level)
        {
            CLog.LogWHeader("MapManager", $"[Map] Show level {level}", "w");
            _mapUI.LevelsDisplay.ShowLevel(level);
            var totalLevel = level;
            var currentIndex = CorrectIndex(level);
            var current = _worldMapParts[currentIndex];
            current.Show();
            current.SetEnemyTerritory();
            current.GlowSetActive(true);
            current.FogSetActive(false);
            current.SpawnLevelEnemies(new SpawnLevelArgs(totalLevel,false));   
            _currentPart = current;
            _camera.SetClosePoint(current.CameraPoint);
            if (level > 0)
            {
                
            }
            var playerPart = SetAllPreviousAsPlayer(level);
            playerPart.ArrowSetActive(true);
            _playerPack.SetPosition(playerPart.PlayerSpawn);
            _playerPack.Spawn();
            
            for (var i = currentIndex + 1; i < _worldMapParts.Count; i++)
            {
                _worldMapParts[i].SetEnemyTerritory();
                _worldMapParts[i].FogSetActive(true);
            }
        }
        
        // Returns previous part, sets all [0-previous] as player
        private WorldMapPart SetAllPreviousAsPlayer(int level)
        {
            var prevLevel = level - 1;
            
            if(prevLevel < 0)
                return null;
            var prevPartIndex = CorrectIndex(prevLevel);
            var prevPart = _worldMapParts[prevPartIndex];
            prevPart.SetPlayerTerritory();
            for (var i = 0; i < prevPartIndex; i++)
                _worldMapParts[i].SetPlayerTerritory();
            return prevPart;
        }
        
        private WorldMapPart GetState()
        {
            var currentIndex = CorrectIndex(GC.PlayerData.LevelTotal);
            var current = _worldMapParts[currentIndex];
            return current;
        }

        public void AnimateToPlayer(int level, float delay)
        {
            Debug.Log($"[Map] Animate to player {level}");
            _mapUI.LevelsDisplay.ShowLevel(level);
            _animatedLevel = level;
            var currentIndex = CorrectIndex(level);
            var current = _worldMapParts[currentIndex];
            current.Show();
            current.SetEnemyTerritory();
            current.GlowSetActive(false);
            current.FogSetActive(false);
            current.SpawnLevelEnemies(new SpawnLevelArgs(level, true));

            var next = _worldMapParts[CorrectIndex(level + 1)];
            next.SetEnemyTerritory();
            next.SpawnLevelEnemies(new SpawnLevelArgs(level + 1, false));
            _currentPart = current;
            _camera.SetClosePoint(current.CameraPoint);
            
            var playerPart = SetAllPreviousAsPlayer(level);
            playerPart.ArrowSetActive(false);
            _playerPack.SetPosition(playerPart.PlayerSpawn);
            _playerPack.Spawn();
            
            StartCoroutine(Delayed(delay, () =>
            {
                _currentPart.AnimateToPlayer(new AnimateArgs()
                {
                    OnComplete = OnComplete,
                    OnEnemyHidden = OnMiddle,
                    ScaleDuration = _unlockAnimScaleTime,
                    FadeDuration = _unlockAnimFadeTime
                });
            }));
        }
        
        private IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            onEnd.Invoke();
        }

        private void OnComplete()
        {
            var bonus = GC.LevelRepository.GetLevel(GC.PlayerData.LevelTotal).Bonus;
            if (bonus == null)
            {
                CLog.LogWHeader("MapManager", "No Bonus", "w");
                return;
            }
            switch (bonus.Type)
            {
                case LevelBonus.BonusType.Egg:
                    StartCoroutine(Delayed(_bonusDelay, () =>
                    {
                        _mapUI.Hide();
                        var bonusUI = GC.UIManager.BonusEggPopup;
                        bonusUI.ShowItem(bonus.Item.Item, OnBonusHidden);
                        var state = GetState();
                        state.CollectBonus();
                    }));
                    break;
            }

        }

        private void OnBonusHidden()
        {
            CLog.LogWHeader("MapManager", "Bonus hidden", "w");
            _mapUI.FadeIn();
        }

        private void OnMiddle()
        {
            var currentIndex = CorrectIndex(_animatedLevel);
            var current = _worldMapParts[currentIndex];
            _playerPack.JumpToCapture(current.PlayerSpawn, jumpDuration);   
            current.ArrowSetActive(true);
            var next = _worldMapParts[CorrectIndex(_animatedLevel+1)];
            _camera.MoveBetweenPoints(current.CameraPoint, next.CameraPoint, _cameraMoveTime);
            _mapUI.LevelsDisplay.AnimateNextLevel(_uiAnimationTime);
        }


        #region Editor
#if UNITY_EDITOR
        public void GetParts()
        {
            _worldMapParts.Clear();
            _worldMapParts = HierarchyUtils.GetFromAllChildren<WorldMapPart>(transform);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void ShowAll()
        {
            foreach (var part in _worldMapParts)
                part.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            foreach (var part in _worldMapParts)
                part.gameObject.SetActive(false);
        }

        public void SetAllAsEnemyTerritory()
        {
            foreach (var part in _worldMapParts)
                part.SetEnemyTerritory();
        }

        public void SetAllAsPlayerTerritory()
        {
            foreach (var part in _worldMapParts)
                part.SetPlayerTerritory();
        }
        
        
        [ContextMenu("Rename states")]
        public void RenameStates()
        {
            var num = 1;
            foreach (var part in _worldMapParts)
            {
                if(part == null)
                    continue;
                part.gameObject.name = $"State {num}";
                part.transform.SetSiblingIndex(num-1);
                num++;
            }
        }
        
        
        [Space(20)]
        [SerializeField] private bool _doDraw;
        [SerializeField] private int _fontSize = 20;

        public void OnDrawGizmos()
        {
            if (!_doDraw)
                return;
            var num = 1;
            var style = new GUIStyle(GUI.skin.GetStyle("Label"));
            style.fontSize = _fontSize;
            style.fontStyle = FontStyle.Bold;
            var oldColor = Handles.color;
            Handles.color = Color.red;
            foreach (var part in _worldMapParts)
            {
                Handles.Label(part.transform.position, $"{num}", style);
                num++;
            }

            Handles.color = oldColor;
        }
        #endif        
    #endregion
    }

}