using System;
using System.Collections.Generic;
using Game.UI.Elements;
using UnityEngine;

namespace Game.UI.Map
{
    public class LevelsMap : MonoBehaviour
    {
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private List<LevelUI> _levelUIs;
        [SerializeField] private CurrentMapLevelPointer _levelPointer;
#if UNITY_EDITOR
        [SerializeField] private int _debugLevel;
#endif
        
        
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

        #if UNITY_EDITOR
        [ContextMenu("Set First n passed")]
        public void SetFirstPassed()
        {
            ShowLevel(_debugLevel);
        }
        #endif

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
            var count = _levelUIs.Count;
            var maxIndex = level;
            if (maxIndex >= count)
                maxIndex = count - 1;
            for (var i = 0; i < maxIndex; i++)
                _levelUIs[i].SetPassed();
            
            _levelUIs[maxIndex].HideAll();
            _levelPointer.ShowAt(_levelUIs[maxIndex].PointerPosition, level + 1);
            
            for (var i = maxIndex; i < count; i++)
                _levelUIs[i].SetLocked();
        }
        
        
        
    }
}