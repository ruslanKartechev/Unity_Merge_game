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
        
        public void Init()
        {
            for (var i = 0; i < _classButtons.Count; i++)
            {
                var btn = _classButtons[i];
                btn.SetCount(_mergeClassUis[i].ItemsCount);
                btn.Deactivate();
                btn.onClicked -= OnMergeClassButton;
                btn.onClicked += OnMergeClassButton;
            }

            foreach (var classUi in _mergeClassUis)
                classUi.Init();
        }
        
        public void ShowDefault()
        {
            _currentClassIndex = 0;
            _classButtons[_currentClassIndex].Activate();
            _mergeClassUis[_currentClassIndex].Show();
        }

        public void UpdateCurrent()
        {
            _classButtons[_currentClassIndex].Activate();
            _mergeClassUis[_currentClassIndex].Show();
        }

        public void ShowFirstWithItemsOrDefault()
        {
            var withItems = ShowFirstWithItems();
            if (withItems == null)
                ShowDefault();
        }
        

        public MergeClassUI ShowFirstWithItems()
        {
            for (var i = 0; i < _mergeClassUis.Count; i++)
            {
                var mc = _mergeClassUis[i];
                if (mc.ItemsCount > 0)
                {
                    ShowByIndex(i);
                    return mc;
                }
            }
            return null;
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