using System.Collections;
using UnityEngine;

namespace Creatives
{
    public class RunawayHuman : MonoBehaviour
    {
        [SerializeField] private bool _autoStart = true;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private Transform _movable;
        [Space(10)]
        [SerializeField] private string _animBool;
        [SerializeField] private string _animKey;
        [SerializeField] private Animator _animator;
        private Coroutine _moving;
        
        private void Start()
        {
            if (_autoStart)
                Begin();
            if(_animator != null)
            {
                _animator.SetBool(_animBool, true);
                _animator.Play(_animKey);
            }
        }

        private void Begin()
        {
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(Moving());
        }

        private IEnumerator Moving()
        {
            while (true)
            {
                _movable.position += _movable.forward * (Time.deltaTime * _speed);
                yield return null;
            }
        }
    }
}