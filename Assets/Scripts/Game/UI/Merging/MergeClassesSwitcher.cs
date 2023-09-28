using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassesSwitcher : MonoBehaviour
    {
        [SerializeField] private List<MergeClassUI> _mergeClassUis; 
        [SerializeField] private List<MergeClassUIButton> _classButtons;
        private int _currentClassIndex = 0;

        public MergeClassUI CurrentClass => _mergeClassUis[_currentClassIndex];
        
        public void ShowDefault()
        {
            for (var i = 0; i < _classButtons.Count; i++)
            {
                var btn = _classButtons[i];
                btn.SetCount(_mergeClassUis[i].ItemsCount);
                btn.Deactivate();
                btn.onClicked -= OnMergeClassButton;
                btn.onClicked += OnMergeClassButton;
            }
            _currentClassIndex = 0;
            _classButtons[_currentClassIndex].Activate();
            _mergeClassUis[_currentClassIndex].Show();
        }

        public void UpdateCurrent()
        {
            _classButtons[_currentClassIndex].Activate();
            _mergeClassUis[_currentClassIndex].Show();
        }

        public void UpdateCurrentCount()
        {
            _classButtons[_currentClassIndex].SetCount(_mergeClassUis[_currentClassIndex].ItemsCount);
        }
        
        public void UpdateCounts()
        {
            for (var i = 0; i < _classButtons.Count; i++)
                _classButtons[i].SetCount(_mergeClassUis[i].ItemsCount);
        }
        
        private void OnMergeClassButton(MergeClassUIButton btn)
        {
            var index = _classButtons.IndexOf(btn);
            ShowByIndex(index);
        }

        public void ShowClass(string classID)
        {
            var index = 0;
            for (var i = 0; i < _mergeClassUis.Count; i++)
            {
                if (_mergeClassUis[i].ClassID == classID)
                {
                    index = i;
                    break;
                }
            }
            if (index == _currentClassIndex)
                return;
            ShowByIndex(index);
        }

        private void ShowByIndex(int index)
        {
            // Debug.Log($"Index: {index}, currentIndex: {_currentClassIndex}");
            _classButtons[_currentClassIndex].Deactivate();
            _mergeClassUis[_currentClassIndex].Hide();
            _classButtons[index].Activate();
            _mergeClassUis[index].Show();
            _currentClassIndex = index;
        }

      
    }
}