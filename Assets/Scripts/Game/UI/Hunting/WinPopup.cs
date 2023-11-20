using System;
using Common.UIPop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private ScalePopup _popup;
        [SerializeField] private PopAnimator _popAnimator;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _nextButton;
        
        public void SetOnClicked(Action next)
        {
            _nextButton.onClick.RemoveAllListeners();
            _nextButton.onClick.AddListener(next.Invoke);
        }
        
        public void Show()
        {
            // _popup.PopUp(null);
            gameObject.SetActive(true);
            _popAnimator.HideAndPlay();
        }

        public void SetAward(float award)
        {
            _text.text = $"+{award}";
        }

        public void Hide(bool animated, Action onEnd = null)
        {
            if (animated)
            {
                _popup.PopDown(onEnd);
            }
            else
            {
                gameObject.SetActive(false);
                onEnd?.Invoke();
            }
        }
        
    }
}