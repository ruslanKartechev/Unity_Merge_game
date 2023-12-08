using System.Collections;
using UnityEngine;

namespace Creatives.Office
{
    public class OfficeKongCamera : MonoBehaviour
    {
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _lookAt;
        [SerializeField] private float _transitionTime;
        [SerializeField] private AnimationCurve _transitionCurve;
        [SerializeField] private bool _preserveY;

        public bool PreserveY
        {
            get => _preserveY;
            set => _preserveY = value;
        }
        private Coroutine _moving;

        #if UNITY_EDITOR
        [ContextMenu("Rotate")]
        public void Rotate()
        {
            if (_lookAt == null)
            {
                Debug.Log("No look at assigned");
                return;
            }
            var tr = transform;
            var rot = Quaternion.LookRotation(_lookAt.position - tr.position);
            tr.rotation = rot;
        }
        #endif
        
        public void Stop()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
        public void StartFollowP1()
        {
            _moving = StartCoroutine(Following(_p1));
        }
        
        public void StartFollowP2()
        {
            _moving = StartCoroutine(Following(_p2));
        }
        
        public void TransitionToP2()
        {
            if(_moving!= null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(Transitioning(_p1, _p2));
        }

        private IEnumerator Following(Transform p)
        {
            var tr = transform;
            while (true)
            {
                var pos = p.position;
                if (PreserveY)
                    pos.y = tr.position.y;
                tr.position = pos;
                var rot = Quaternion.LookRotation(_lookAt.position - tr.position);
                tr.rotation = rot;
                yield return null;
            }   
        }

        private IEnumerator Transitioning(Transform p1, Transform p2)
        {
            var tr = transform;
            var time = _transitionTime;
            var elapsed = Time.deltaTime;
            var t = elapsed  / time;
            while (t <= 1f)
            {
                t = elapsed / time;
                tr.position = Vector3.Lerp(p1.position, p2.position, t);
                var rot = Quaternion.LookRotation(_lookAt.position - tr.position);
                tr.rotation = rot;
                elapsed += Time.deltaTime * _transitionCurve.Evaluate(t);
                yield return null;
            }
            yield return Following(p2);
        }

    }
}