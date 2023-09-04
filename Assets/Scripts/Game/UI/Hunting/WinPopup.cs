using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private float _showTime;
        [SerializeField] private Transform _scalable;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Ease _popEase;
        private Action _onClicked;
        
        
        public void SetOnClicked(Action onClicked)
        {
            _onClicked = onClicked;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => { _onClicked.Invoke();});
        }
        
        public void Show()
        {
            _scalable.localScale = Vector3.zero;
            gameObject.SetActive(true);
            _scalable.DOScale(Vector3.one, _showTime).SetEase(_popEase);
        }

        public void SetAward(float award)
        {
            _text.text = $"{award}";
        }

        public void Hide(bool animated, Action onEnd = null)
        {
            if(animated)
                _scalable.DOScale(Vector3.zero, _showTime).SetEase(_popEase).OnComplete(() => { onEnd.Invoke();});
            else
            {
                gameObject.SetActive(false);
                onEnd.Invoke();
            }
        }

    }
}