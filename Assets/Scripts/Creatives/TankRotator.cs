using System.Collections;
using UnityEngine;

namespace Creatives
{
    public class TankRotator : MonoBehaviour
    {
        [SerializeField] private Transform _rotatable;
        private Coroutine _working;
        
        public void RotateToPoint(Transform point, float time)
        {
            if(_working != null)
                StopCoroutine(_working);
            _working = StartCoroutine(Rotating(point, time));
        }

        private IEnumerator Rotating(Transform point, float time)
        {
            var elapsed = 0f;
            var rot1 = _rotatable.rotation;
            var rot2 = Quaternion.LookRotation(point.position - _rotatable.position);
            while (elapsed <= time)
            {
                rot2 = Quaternion.LookRotation(point.position - _rotatable.position);
                _rotatable.rotation = Quaternion.Lerp(rot1, rot2, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _rotatable.rotation = rot2;

        }
    }
}