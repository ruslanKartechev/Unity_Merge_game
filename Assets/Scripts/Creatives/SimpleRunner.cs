using System;
using System.Collections;
using UnityEngine;

namespace Creatives
{
    public class SimpleRunner : MonoBehaviour
    {
        [SerializeField] private bool _autoStart;
        [SerializeField] private Transform _movable;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private float _speed;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _key;

        private void Start()
        {
            if(_autoStart) 
                Begin();
        }

        public void Begin()
        {
            if (_movable == null)
                _movable = transform;
            if (_animator != null)
            {
                _animator.Play(_key);
                _animator.SetBool("isRunning", true);
            }
            StopAllCoroutines();
            StartCoroutine(RunningForward());
        }

        private IEnumerator RunningForward()
        {
            while (true)
            {
                _movable.position += _movable.forward * (Time.deltaTime * _speed);
                yield return null;
            }
        }
        
        private IEnumerator RunToPoint()
        {
            var elapsed  = 0f;
            var startPos = _movable.position;
            var endPos = _targetPoint.position;
            var time = (endPos - startPos).magnitude / _speed;
            while (elapsed <= time)
            {
                _movable.position = Vector3.Lerp(startPos, endPos, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}