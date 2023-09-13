using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassesSwitcher : MonoBehaviour
    {
        [SerializeField] private List<MergeClassUI> _mergeClassUis; 
        [SerializeField] private List<MergeClassUIButton> _classButtons;
        private int _currentClassIndex = 0;

        public void ShowDefault()
        {
            foreach (var btn in _classButtons)
            {
                btn.Deactivate();
                btn.onClicked -= OnMergeClassButton;
                btn.onClicked += OnMergeClassButton;
            }
            _currentClassIndex = 0;
            _classButtons[_currentClassIndex].Activate();
            _mergeClassUis[_currentClassIndex].Show();
        }

        private void OnMergeClassButton(MergeClassUIButton btn)
        {
            var index = _classButtons.IndexOf(btn);
            _classButtons[_currentClassIndex].Deactivate();
            _classButtons[index].Activate();
            _mergeClassUis[index].Show();
            _mergeClassUis[_currentClassIndex].Hide();
            _currentClassIndex = index;
        }
    }
}