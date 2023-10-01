using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class CircleRotator : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private float _speed = 10;
        [SerializeField] private Transform _center;
        [SerializeField] private Vector2 _verticalMoveLimits;
        [SerializeField] private float _verticalSpeed;
        private Coroutine _working;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_center == null)
                _center = transform;
        }
#endif
        
        [ContextMenu("Begin")]
        public void Begin()
        {
            // Debug.Log($"CIRCLE ROT BEGAN {transform.parent.gameObject.name}");
            Stop();
            _speed *= (UnityEngine.Random.Range(0f, 1f) >= .5f ? -1f : 1f);
            _working = StartCoroutine(Working());
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        public void RotateToStrait(float time)
        {
            Stop();
            _working = StartCoroutine(RotatingTo(Quaternion.identity, time));
        }

        private IEnumerator RotatingTo(Quaternion localRotation, float time)
        {
            var startRots = new List<Quaternion>(_targets.Count);
            var elapsed = 0f;
            for (var i = 0; i < _targets.Count; i++)
                startRots.Add(_targets[i].localRotation);
            while (elapsed <= time)
            {
                var t = elapsed / time;
                for (var i = 0; i < _targets.Count; i++)
                    _targets[i].localRotation = Quaternion.Lerp(startRots[i], localRotation, t);           
                elapsed += Time.deltaTime;
                yield return null;
            }
            for (var i = 0; i < _targets.Count; i++)
                _targets[i].localRotation = localRotation;
        }
        

        private IEnumerator Working()
        {
            var targets = GetTargets();
            while (true)
            {
                var rotAmount = Time.deltaTime * _speed;
                foreach (var movable in targets)
                {
                    
                    var tr = movable.target;
                    var oldPos = tr.position;
                    var vec = tr.position - _center.position;
                    vec = Quaternion.Euler(0f, rotAmount, 0f) * vec;
                    var newPos = _center.position + vec;
                    
                    tr.position = newPos;
                    tr.rotation = Quaternion.LookRotation(newPos - oldPos);
             
                    if (movable.IsGointUp)
                    {
                        movable.RelativeLocalY += Time.deltaTime * _verticalSpeed;
                        movable.ApplyY();
                        if (movable.RelativeLocalY >= _verticalMoveLimits.y)
                            movable.IsGointUp = false;
                    }
                    else
                    {
                        movable.RelativeLocalY -= Time.deltaTime * _verticalSpeed;
                        movable.ApplyY();
                        if (movable.RelativeLocalY <= _verticalMoveLimits.x)
                            movable.IsGointUp = true;
                    }
                }
                yield return null;
            }
        }

        private IList<Movable> GetTargets()
        {
            var list = new List<Movable>(_targets.Count);
            foreach (var tt in _targets)
            {
                var movable = new Movable(tt);
                movable.RandomY(_verticalMoveLimits);
                list.Add(movable);
            }
            return list;
        }

        private class Movable
        {
            public Transform target;
            public bool IsGointUp;
            private float _startY;

            public float RelativeLocalY;

            public void ApplyY()
            {
                var pos = target.localPosition;
                pos.y = RelativeLocalY + _startY;
                target.localPosition = pos;
            }
            
            public float LocalY => target.localPosition.y;

            public Movable(Transform target)
            {
               this.target = target;
               IsGointUp = (UnityEngine.Random.Range(0f, 1f) >= 0.5f);
               _startY = target.localPosition.y;
            }

            public void RandomY(Vector2 limits)
            {
                RelativeLocalY = limits.Random();
                ApplyY();
            }
        }
    }
}