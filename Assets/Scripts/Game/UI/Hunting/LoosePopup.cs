using System;
using Common.UIPop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hunting
{
    public class LoosePopup : MonoBehaviour
    {
        [SerializeField] private ScalePopup _popup;
        [SerializeField] private PopAnimator _popAnimator;
        [Space(10)]
        [SerializeField] private Button _restartFromMergeButton;
        [SerializeField] private Button _replayButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        private Action _onClicked;
        
        public void SetOnClicked(Action onExit, Action onReplay)
        {
            _restartFromMergeButton.onClick.RemoveAllListeners();
            _replayButton.onClick.RemoveAllListeners();
            
            _restartFromMergeButton.onClick.AddListener(onExit.Invoke);
            _replayButton.onClick.AddListener(onReplay.Invoke);
        }

        public void SetReward(float reward)
        {
            _rewardText.text = $"+{reward}";   
        }

        public void Show()
        {
            _popAnimator.HideAndPlay();
            // _popup.PopUp(null);
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