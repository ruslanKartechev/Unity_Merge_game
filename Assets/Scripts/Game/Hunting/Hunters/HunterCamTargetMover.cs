using System.Collections;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HunterCamTargetMover : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        [SerializeField] private Transform _followTarget;
        private const float lerpXZ = 0.2f;
        private const float lerpY = 0.02f;
        
        public void Follow()
        {
            _movable.parent = null;
            StartCoroutine(Moving());
        }

        private IEnumerator Moving()
        {
            var offset = (_movable.position - _followTarget.position);
            while (true)
            {
                var cp = _movable.position;
                var tp = (_followTarget.position + offset);
                var pos = Vector3.Lerp(cp, tp, lerpXZ);
                pos.y = Mathf.Lerp(cp.y, tp.y, lerpY);
                _movable.position = pos;
                yield return null;
            }
        }
    }
}