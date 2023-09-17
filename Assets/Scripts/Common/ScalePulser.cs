using System;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    public class ScalePulser : MonoBehaviour
    {
        [SerializeField] private float _magnitude;
        [SerializeField] private float _time;
        [SerializeField] private bool _autoStart;
        [SerializeField] private Transform _target;
        private Sequence _sequence;

        private void OnEnable()
        {
            if (_autoStart == false)
                return;
            Begin();
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Begin()
        {
       
            Stop();
            _sequence = DOTween.Sequence();
            _target.localScale = Vector3.one * (1 - _magnitude);
            _sequence.Append(_target.DOScale(Vector3.one * (1 + _magnitude), _time));
            _sequence.Append(_target.DOScale(Vector3.one * (1 - _magnitude), _time));
            _sequence.SetLoops(-1);
        }

        public void Stop()
        {
            _sequence?.Kill();
   
        }
    }
}