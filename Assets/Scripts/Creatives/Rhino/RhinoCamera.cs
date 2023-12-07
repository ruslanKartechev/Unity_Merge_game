using System.Collections;
using UnityEngine;

namespace Creatives.Rhino
{
    public class RhinoCamera : MonoBehaviour
    {
        [SerializeField] private bool _autoStart = true;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _movable;
        [Header("OnBumb")]
        [SerializeField] private Vector3 _bump;
        [SerializeField] private float _bumpToTime = 1f;
        [SerializeField] private float _bumpFromTime = 1f;
        [SerializeField] private AnimationCurve _bumpToCurve;
        [SerializeField] private AnimationCurve _bumpFromCurve;
        [Header("Regular")] 
        [SerializeField] private float _backupSpeed;
        [SerializeField] private Vector3 _backupDir;
        public Transform Target => _target;
        private Vector3 _offset;
        private Coroutine _working;
        
        private void Start()
        {
            if(_autoStart)
                Follow();
        }

        public void Follow()
        {
            _offset = (_movable.position - Target.position);
            Stop();
            _working = StartCoroutine(Following());
        }

        public void Bump()
        {
            Stop();
            _working = StartCoroutine(Bumping());
        }

        private void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private IEnumerator Following()
        {
            var elapsed = 0f;
            var t = 0f;
            while (true)
            {
                _offset += _backupDir * (_backupSpeed * Time.deltaTime);
                var pos = Target.position + _offset;
                // pos.y = _movable.position.y;
                _movable.position = pos;
                yield return null;
            }
        }

        private IEnumerator Bumping()
        {
            var time = _bumpToTime;
            var elapsed = Time.deltaTime;
            var t = elapsed / time;
            var startOff = _offset;
            var endOff = _offset - _bump;
            Vector3 pos;
            while (t <= 1)
            {
                SetPos2(startOff, endOff, t);
                t = elapsed / time;
                elapsed += Time.deltaTime * _bumpToCurve.Evaluate(t);
                yield return null;
            }
            SetPos2(startOff, endOff, 1);
            
            time = _bumpFromTime;
            t = elapsed = 0f;
            while (t <= 1)
            {
                SetPos2(endOff, startOff, t);
                t = elapsed / time;
                elapsed += Time.deltaTime * _bumpFromCurve.Evaluate(t);
                yield return null;
            }
            SetPos2(endOff, startOff, 1);
            pos.y = _movable.position.y;
            Follow();

            
            void SetPos2(Vector3 start, Vector3 end, float t)
            {
                var pos = Vector3.Lerp(start, end, t)+ Target.position;
                // pos.y = _movable.position.y;
                _movable.position = pos;
            }
            
        }        
    }
}