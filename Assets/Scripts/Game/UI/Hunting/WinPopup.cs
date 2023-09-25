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
        [SerializeField] private RectTransform _moveTarget;
        [SerializeField] private Vector2 _positionHidden;
        [SerializeField] private Vector2 _positionShown;
        [SerializeField] private Ease _popEase;
        [SerializeField] private Ease _hideEase;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        private Action _onClicked;
        
        
        public void SetOnClicked(Action onClicked)
        {
            _onClicked = onClicked;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => { _onClicked.Invoke();});
        }
        
        public void Show()
        {
            _moveTarget.anchoredPosition = _positionHidden;
            _moveTarget.DOAnchorPosY(_positionShown.y, _showTime).SetEase(_popEase);
            _moveTarget.localScale = new Vector3(0.5f, 0.76f, 1f);
            _moveTarget.DOScale(Vector3.one, _showTime).SetEase(_popEase);
            
            gameObject.SetActive(true);
        }

        public void SetAward(float award)
        {
            _text.text = $"{award}";
        }

        public void Hide(bool animated, Action onEnd = null)
        {
            if (animated)
            {
                _moveTarget.DOScale(new Vector3(0.5f, 0.76f, 1f), _showTime).SetEase(_hideEase);
                _moveTarget.DOAnchorPos(_positionHidden, _showTime).SetEase(_hideEase).OnComplete(() => { onEnd.Invoke();});
            }
            else
            {
                gameObject.SetActive(false);
                onEnd.Invoke();
            }
        }

    }
}