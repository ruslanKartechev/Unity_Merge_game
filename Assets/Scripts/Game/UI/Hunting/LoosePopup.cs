using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class LoosePopup : MonoBehaviour
    {
        [SerializeField] private float _showTime;
        [SerializeField] private RectTransform _moveTarget;
        [SerializeField] private Vector2 _positionHidden;
        [SerializeField] private Vector2 _positionShown;
        [SerializeField] private Ease _popEase;
        [SerializeField] private Ease _hideEase;
        [Space(10)]
        [SerializeField] private Button _button;
        private Action _onClicked;
        
        public void SetOnClicked(Action onClicked)
        {
            _onClicked = onClicked;
            _button.onClick.RemoveListener(OnClick);
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick() => _onClicked.Invoke(); 

        public void Show()
        {
            _moveTarget.anchoredPosition = _positionHidden;
            _moveTarget.DOAnchorPos(_positionShown, _showTime).SetEase(_popEase);
            gameObject.SetActive(true);
        }
        
        public void Hide(bool animated, Action onEnd = null)
        {
            if(animated)
                _moveTarget.DOAnchorPos(_positionHidden, _showTime).SetEase(_hideEase).OnComplete(() => { onEnd.Invoke();});
            else
            {
                gameObject.SetActive(false);
                onEnd.Invoke();
            }
        }
        

    }
}