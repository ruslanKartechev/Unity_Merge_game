using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterListenerRotate : HunterListener
    {
        [SerializeField] private Transform _rotatable;
        [SerializeField] private float _rotSpeed;
        private Coroutine _rotating;
        
        
        public override void OnAttack()
        {
            _rotSpeed *= UnityEngine.Random.Range(0f, 1f) >= 0.5 ? 1 : -1;
            _rotating = StartCoroutine(Rotating());
        }

        public override void OnFall()
        {
            if(_rotating != null)
                StopCoroutine(_rotating);
        }

        private IEnumerator Rotating()
        {
            while (true)
            {
                var angles = _rotatable.localEulerAngles;
                angles.z += Time.deltaTime * _rotSpeed;
                _rotatable.localEulerAngles = angles;
                yield return null;
            }
        }
    }
}