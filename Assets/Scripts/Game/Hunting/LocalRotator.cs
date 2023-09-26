using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class LocalRotator : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _rotationCurve;
        [SerializeField] private Transform _rotatable;
        private Coroutine _moving;

        private void Awake()
        {
            if (_rotatable == null)
                _rotatable = transform;
        }

        public void RotateTo(Quaternion localRot, float time)
        {
            Stop();
            _moving = StartCoroutine(Rotation(localRot, time));
        }

        public void Stop()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
        private IEnumerator Rotation(Quaternion targetRot, float time)
        {
            var elapsed = 0f;
            var startRot = _rotatable.localRotation;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                _rotatable.localRotation = Quaternion.Lerp(startRot, targetRot, t);
                elapsed += Time.deltaTime * _rotationCurve.Evaluate(t);
                yield return null;
            }
            _rotatable.localRotation = targetRot;
        }   
    }
}