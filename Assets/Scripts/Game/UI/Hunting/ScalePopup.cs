using System;
using DG.Tweening;
using UnityEngine;

namespace Game.UI.Hunting
{
    public class ScalePopup : MonoBehaviour
    {
        [SerializeField] private float _dropTime;
        [SerializeField] private float _backTime;
        [Space(5)]
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private Vector2 _dropPos;
        [SerializeField] private Vector2 _finalPosition;
        [Space(5)]
        [SerializeField] private Vector3 _startScale;
        [Space(10)]
        [SerializeField] private RectTransform _scalable;
        [SerializeField] private Ease _ease1;
        [SerializeField] private Ease _ease2;
        
        private Sequence _sequence;
        
        public void PopUp(Action onDone)
        {
            _scalable.DOKill();
            
            _sequence = DOTween.Sequence();
            _scalable.localScale = Vector3.one;
            _scalable.anchoredPosition = _startPosition;
            _sequence.Append(_scalable.DOAnchorPos(_dropPos, _dropTime).SetEase(_ease1));
            _sequence.Append(_scalable.DOAnchorPos(_finalPosition, _backTime).SetEase(_ease2));
            
            var seq2 = DOTween.Sequence();
            seq2.Append(_scalable.DOScale(_startScale, _dropTime).SetEase(_ease1));
            seq2.Append(_scalable.DOScale(Vector3.one, _backTime).SetEase(_ease2));
            
            _sequence.OnComplete(() => {onDone?.Invoke();});
        }
        
        public void PopDown(Action onDone)
        {
            _scalable.DOKill();
            
            _sequence = DOTween.Sequence();
            _scalable.anchoredPosition = _finalPosition;
            _sequence.Append(_scalable.DOAnchorPos(_dropPos, _backTime).SetEase(_ease2));
            _sequence.Append(_scalable.DOAnchorPos(_startPosition, _dropTime).SetEase(_ease1));
            
            var seq2 = DOTween.Sequence();
            seq2.Append(_scalable.DOScale(Vector3.one * 1.1f, _backTime).SetEase(_ease2));
            seq2.Append(_scalable.DOScale(_startScale, _dropTime).SetEase(_ease1));
            
            _sequence.OnComplete(() => {onDone?.Invoke();});
        }
        
        #if UNITY_EDITOR
        [ContextMenu("PopUpTest")]
        public void PopUpTest()
        {
            PopUp(() => {Debug.Log("Popup test over");});
        }
        
        [ContextMenu("PopDownTest")]
        public void PopDownTest()
        {
            PopDown(() => {Debug.Log("Popup test over");});
        }


        #endif
    }
}