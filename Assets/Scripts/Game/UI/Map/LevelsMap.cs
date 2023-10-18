using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI.Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.Map
{
    public class LevelsMap : MonoBehaviour
    {
        private const float FadeTime = .5f;
        private const float MoveTime = .5f;   
        
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private List<LevelUI> _levelUIs;
        [SerializeField] private CurrentMapLevelPointer _levelPointer;
        [SerializeField] private Button _continueButton;  
        
        
        
        [ContextMenu("Init Levels")]
        public void InitLevels()
        {
            for (var i = 0; i < _levelUIs.Count; i++)
            {
                _levelUIs[i].SetLevel(i+1);
                #if UNITY_EDITOR
                if (Application.isPlaying == false)
                    UnityEditor.EditorUtility.SetDirty(_levelUIs[i].gameObject);
                #endif
            }
        }

        public void ShowCurrentLevel()
        {
            var currentLevel = GC.PlayerData.LevelTotal;
            if (currentLevel >= _levelUIs.Count)
                currentLevel = _levelUIs.Count - 1;
            ShowLevel(currentLevel);
        }
        
        public void ShowLevel(int level)
        {
            _levelDisplay.SetCurrent();
            var stateIndex = CorrectIndex(level);
            ShowGreenUpTo(stateIndex);
            
            _levelUIs[stateIndex].HideAll();
            _levelPointer.ShowAt(_levelUIs[stateIndex].PointerPosition, level + 1);
        }
        
        public void SetOnContinue(UnityAction onContinue)
        {
            if (_continueButton == null)
            {
                Debug.LogError("NO continue button");
                return;
            }
            _continueButton.onClick.AddListener(onContinue);
        }
        
        public void MoveToLevel(int level)
        {
            var stateIndex = CorrectIndex(level);
            ShowGreenUpTo(stateIndex);
            StartCoroutine(Moving(stateIndex, level));
        }

        private void ShowGreenUpTo(int stateIndex)
        {
            var count = _levelUIs.Count;
            for (var i = 0; i < stateIndex; i++)
                _levelUIs[i].SetPassed();
            for (var i = stateIndex; i < count; i++)
                _levelUIs[i].SetLocked();   
        }

        private IEnumerator Moving(int stateIndex, int levelMax)
        {
            var prevPoint = _levelUIs[stateIndex - 1];
            var currentPoint = _levelUIs[stateIndex]; 
            _levelPointer.SetPosition(prevPoint.PointerPosition);
            _levelPointer.SetLevel(levelMax);
            _levelPointer.ScaleDown();
            prevPoint.FadeIn(FadeTime);
            yield return new WaitForSeconds(FadeTime * 1.1f);
            _levelPointer.SetLevel(levelMax + 1);
            _levelPointer.MoveFromTo(prevPoint.PointerPosition, currentPoint.PointerPosition, MoveTime);
        }
        
        private int CorrectIndex(int level)
        {
            var count = _levelUIs.Count;
            var maxIndex = level;
            if (maxIndex >= count)
                maxIndex = count - 1;
            return maxIndex;
        }
        
        
        
#if UNITY_EDITOR
        [Space(22)]  
        [SerializeField] private int _debugLevel;
        [ContextMenu("Set First n passed")]
        public void SetFirstPassed()
        {
            ShowLevel(_debugLevel);
        }
#endif
    }
}