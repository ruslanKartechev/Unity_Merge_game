using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class Captive : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        [SerializeField] private float _moveSpeed;
        [Header("Run Waypoints"), Space(10)]
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _p3;
        [Space(10)]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _runAnimator;
        [Space(10)] 
        [SerializeField] private List<ParticleSystem> _particles;


#if UNITY_EDITOR
        public bool drawGizmos = true;
        public Color gizmosColor;
        
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = gizmosColor;
            var count = 20;
            var oldP = _p1.position;
            var upOffset = 0.2f;
            for (var i = 1; i < count; i++)
            {
                var t = (float)i / (count - 1);
                var p = Bezier.GetPosition(_p1.position, _p2.position, _p3.position, t);
                Gizmos.DrawLine(oldP + Vector3.up * upOffset, p + Vector3.up * upOffset);
                oldP = p;
            }
            Gizmos.color = oldColor;
        }
#endif

        public void RunAway()
        {
            PlayParticles();
            _animator.Play(_runAnimator);
            StartCoroutine(Moving());
        }

        private void PlayParticles()
        {
            var random = _particles.Random();
            random.gameObject.SetActive(true);
            random.Play();
        }
        
        private IEnumerator Moving()
        {
            var distance = (_p2.position - _p1.position).magnitude + (_p3.position - _p2.position).magnitude;
            var time = distance / _moveSpeed;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var p = Bezier.GetPosition(_p1.position, _p2.position, _p3.position, t);
                var dir = p - _movable.position;
                dir.y = 0;
                _movable.rotation = Quaternion.LookRotation(dir);
                _movable.position = p;
                elapsed += Time.deltaTime;
                yield return null;
            }
            yield return null;
            gameObject.SetActive(false);
        }
    }
}