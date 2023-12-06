using System;
using System.Collections;
using UnityEngine;

namespace Creatives.Firemen
{
    public class JumpKongCamera : MonoBehaviour
    {
        [SerializeField] private bool _autoSetP1 = true;
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _movable;
        [SerializeField] private float _moveTime;
        [SerializeField] private AnimationCurve _curve;

        private void Start()
        {
            if (_autoSetP1)
            {
                _movable.SetPositionAndRotation(_p1.position, _p1.rotation);
            }
        }

        public void MoveToP2()
        {
            StartCoroutine(MovingToP2());
        }


        private IEnumerator MovingToP2()
        {
            var pos1 = _movable.position;
            var pos2 = _p2.position;
            var rot1 = _movable.rotation;
            var rot2 = _p2.rotation;
            var elapsed = 0f;
            var time = _moveTime;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var pos = Vector3.Lerp(pos1, pos2, t);
                var rot = Quaternion.Lerp(rot1, rot2, t);
                _movable.SetPositionAndRotation(pos, rot);
                elapsed += Time.deltaTime * _curve.Evaluate(t);
                yield return null;
            }
            _movable.SetPositionAndRotation(pos2, rot2);
        }

    }
}