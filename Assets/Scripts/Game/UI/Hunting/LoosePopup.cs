using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class LoosePopup : MonoBehaviour
    {
        [SerializeField] private ScalePopup _popup;
        [Space(10)]
        [SerializeField] private Button _restartFromMergeButton;
        [SerializeField] private Button _replayButton;

        private Action _onClicked;
        
        public void SetOnClicked(Action onExit, Action onReplay)
        {
            _restartFromMergeButton.onClick.RemoveAllListeners();
            _replayButton.onClick.RemoveAllListeners();
            
            _restartFromMergeButton.onClick.AddListener(onExit.Invoke);
            _replayButton.onClick.AddListener(onReplay.Invoke);
        }

        public void Show()
        {
            _popup.PopUp(null);
            gameObject.SetActive(true);
        }
        
        public void Hide(bool animated, Action onEnd = null)
        {
            if(animated)
                _popup.PopDown(onEnd);
            else
            {
                gameObject.SetActive(false);
                onEnd?.Invoke();
            }
        }
        

    }
}