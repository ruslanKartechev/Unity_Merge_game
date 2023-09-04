using DG.Tweening;
using UnityEngine;

namespace Game.UI.Elements
{
    public class BouncyPrompt : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _bounceScale;
        [SerializeField] private float _bounceTime;
        [SerializeField] private int _bouncesCount;
        [SerializeField] private float _hideTime;

        private Sequence _sequence;
        
        
        public void Show()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            var seq1 = DOTween.Sequence();
            _target.localScale = Vector3.one * (1 + _bounceScale);
            _target.gameObject.SetActive(true);
            seq1.Append(_target.DOScale(Vector3.one * (1 - _bounceScale), _bounceTime));
            seq1.Append(_target.DOScale(Vector3.one * (1 + _bounceScale), _bounceTime));
            seq1.SetLoops(_bouncesCount);
            _sequence.Append(seq1);
            _sequence.Append(_target.DOScale(Vector3.zero, _hideTime).OnComplete(() => {gameObject.SetActive(false);}));
            
        }

        public void Hide()
        {
            _target.gameObject.SetActive(false);
        }   
        
    }
}