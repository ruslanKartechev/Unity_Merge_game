using System.Collections;
using Common;
using Game.Hunting;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongJumper : MonoBehaviour
    {
        public float jumpTime;
        public Animator animator;
        public string jumpKey;
        public Transform movable;
        public float lerpRotSpeed = .2f;
        [SerializeField] private ParticleSystem _particles;

        
        public void Jump(AimPath path)
        {
            if (_particles != null)
            {
                _particles.transform.parent = null;
                _particles.gameObject.SetActive(true);
                _particles.Play();
            }
            animator.SetTrigger(jumpKey);
            StartCoroutine(Working(path));
        }

        private IEnumerator Working(AimPath path)
        {
            var elapsed = 0f;
            while (elapsed <= jumpTime)
            {
                var t = elapsed / jumpTime;
                var p = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var lookVec = p - movable.position;
                lookVec.y = 0;
                movable.position = p;
                var targetRot = Quaternion.LookRotation(lookVec);
                movable.rotation = Quaternion.Lerp(movable.rotation, targetRot, lerpRotSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}