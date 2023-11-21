using System;
using System.Collections;
using Common;
using Common.UIPop;
using Game.Merging;
using Game.UI.Merging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hunting
{
    public class BossLevelWinPopup : MonoBehaviour
    {
        [SerializeField] private ScalePopup _popup;
        [SerializeField] private PopAnimator _popAnimator;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _nextButton;
        [SerializeField] private ScalePulser _eggPulser;
        [SerializeField] private SuperEggUI _superEggUI;
        [SerializeField] private float _pulserDelay = 1f;
        
        public void SetOnClicked(Action next)
        {
            _nextButton.onClick.RemoveAllListeners();
            _nextButton.onClick.AddListener(next.Invoke);
        }
        
        public void Show(SuperEgg egg)
        {
            _superEggUI.Show(egg);
            _popup.PopUp(null);
            gameObject.SetActive(true);
            _popAnimator.HideAndPlay();
            StartCoroutine(DelayedPulser());
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

        private IEnumerator DelayedPulser()
        {
            yield return new WaitForSeconds(_pulserDelay);
            _eggPulser.Begin();
        }
    }
}