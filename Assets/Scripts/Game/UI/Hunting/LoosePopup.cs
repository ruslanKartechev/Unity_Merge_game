using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class LoosePopup : MonoBehaviour
    {
        [SerializeField] private float _showTime;
        [SerializeField] private Transform _scalable;
        [SerializeField] private Button _button;
        [SerializeField] private Ease _popEase;
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
            gameObject.SetActive(true);
            _scalable.localScale = Vector3.zero;
            _scalable.DOScale(Vector3.one, _showTime).SetEase(_popEase);
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