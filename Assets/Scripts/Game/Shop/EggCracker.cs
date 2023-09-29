using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Shop
{
    public class EggCracker : MonoBehaviour
    {
        [SerializeField] private Transform _rotatable;
        [SerializeField] private Transform _jumpTarget;
        [SerializeField] private Vector2 _randomRotLimits;
        [SerializeField] private float _lerpRotSpeed = .1f;
        [Space(10)] 
        [SerializeField] private float _YpunchDuraiton = .1f;
        [SerializeField] private float _YpunchMagn = .1f;
        
        [Space(10)]
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private List<Transform> _targets_1;
        [SerializeField] private List<Transform> _targets_2;
        
        [SerializeField] private float _scaleMin;
        [SerializeField] private float _stepsCount;
        private Coroutine _cracking;
        private Coroutine _tilting;
        private float _targetAngle;

        private bool _isWorking;
        public bool IsWorking => _isWorking;
        
#if UNITY_EDITOR
        [ContextMenu("Reset Scale")]
        public void ResetScale()
        {
            foreach (var tr in _targets)
            {
                tr.localScale = Vector3.one;
            }
        }

        [ContextMenu("GenerateRandomTargets")]
        public void GenerateRandomTargets()
        {
            _targets_1.Clear();
            _targets_2.Clear();
                
            foreach (var tr in _targets)
            {
                var doAdd = UnityEngine.Random.Range(0f, 1f) > 0.5f;
                if(doAdd)
                    _targets_1.Add(tr);
                else
                    _targets_2.Add(tr);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        
        public void Crack(float duration)
        {
            Stop();
            _cracking = StartCoroutine(Cracking(duration));
            _tilting = StartCoroutine(Tilting());
        }

        public void Stop()
        {
            if(_cracking != null)
                StopCoroutine(_cracking);
            if(_tilting != null)
                StopCoroutine(_tilting);
        }

        public void Reset()
        {
            foreach (var tr in _targets)
                tr.localScale = Vector3.one;
            _rotatable.localRotation = Quaternion.identity;
        }
        
        private void RandomRot()
        {
            _targetAngle = _randomRotLimits.Random() * Mathf.Sign(_targetAngle) * -1f;
        }
        
        
        private IEnumerator Cracking(float duration)
        {
            _isWorking = true;
            var elapsed = 0f;
            var stepDelay = duration / _stepsCount;
            var step = 0;
            var scaleStep = (1f - _scaleMin) / _stepsCount;
            RandomRot();
            while (step < _stepsCount)
            {
                yield return new WaitForSeconds(stepDelay);
                RandomRot();
                yield return Punching();
                step++;
                foreach (var tr in _targets)
                    tr.localScale = Vector3.one * (1 - scaleStep * step);
            }
            if(_tilting != null)
                StopCoroutine(_tilting);
            _isWorking = false;
        }

        private IEnumerator Punching()
        {
            var elapsed = 0f;
            var time = _YpunchDuraiton * 0.3f;
            var pos = _rotatable.localPosition;
            var target = _jumpTarget;
            while (elapsed <= time)
            {
                pos = target.localPosition;
                pos.y = Mathf.Lerp(0f, _YpunchMagn, elapsed / time);
                target.localPosition = pos;
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            elapsed = 0f;
            time = _YpunchDuraiton * 0.7f;
            while (elapsed <= time)
            {
                pos = target.localPosition;
                pos.y = Mathf.Lerp(_YpunchMagn, 0f, elapsed / time);
                target.localPosition = pos;
                elapsed += Time.deltaTime;
                yield return null;
            }
            pos = target.localPosition;
            pos.y = 0f;
            target.localPosition = pos;
        }
        
        private IEnumerator Tilting()
        {
            while (true)
            {
                var rot = Quaternion.Euler(new Vector3(0f, 0f, _targetAngle));
                _rotatable.localRotation = Quaternion.Lerp(_rotatable.localRotation, rot, _lerpRotSpeed);
                yield return null;
            }
        }

    }
}