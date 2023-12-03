using System;
using System.Collections;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongCameraFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _lerpX;

        public Transform Target => _target;
        private void Awake()
        {
            Begin();   
        }

        public void Begin()
        {
            Stop();
            StartCoroutine(Following());
        }

        public void Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator Following()
        {
            var offsetZ = transform.position.z - _target.position.z;
            while (true)
            {
                var pos = transform.position;
                var targetPos = Target.position;
                pos.z = targetPos.z + offsetZ;
                pos.x = Mathf.Lerp(pos.x, targetPos.x, _lerpX);
                transform.position = pos;
                yield return null;
            }
        }
    }
}