using DG.Tweening;
using UnityEngine;

namespace Common.UIPop
{
    public class SimplePopElement : PopElement
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        
        public override float Delay => _delay;
        public override float Duration => _duration;
        
        public override void ScaleUp()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _duration).SetEase(_ease).SetDelay(_duration);
        }
        
    }
}