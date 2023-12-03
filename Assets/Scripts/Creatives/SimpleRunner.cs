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
            StopAllCoroutines();
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            var elapsed  = 0f;
            var startPos = _movable.position;
            var endPos = _targetPoint.position;
            var time = (endPos - startPos).magnitude / _speed;
            _animator?.Play(_key);
            while (elapsed <= time)
            {
                _movable.position = Vector3.Lerp(startPos, endPos, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}