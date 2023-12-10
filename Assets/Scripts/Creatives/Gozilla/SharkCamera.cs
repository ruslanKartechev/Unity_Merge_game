using System.Collections;
using UnityEngine;

namespace Creatives.Gozilla
{
    public class SharkCamera : MonoBehaviour
    {
        [SerializeField] private float _transitionTime;
        [SerializeField] private Transform _follow;
        [SerializeField] private Transform _lookAt;
        [SerializeField] private AnimationCurve _transitionCurve;
        [SerializeField] private bool _preserveY;
        private Coroutine _moving;
        public bool RotateWhenFollow { get; set; } = true;

        public Transform FollowTarget
        {
            get => _follow;
            set => _follow = value;
        }

        public Transform LookAtTarget
        {
            get => _lookAt;
            set => _lookAt = value;
        }
        
        public bool PreserveY
        {
            get => _preserveY;
            set => _preserveY = value;
        }

#if UNITY_EDITOR
        [ContextMenu("Position and Rotate")]
        public void EditorPositionRotate()
        {
            if (_lookAt == null)
            {
                Debug.Log("No look at assigned");
                return;
            }
            var tr = transform;
            tr.position = _follow.position;
            var rot = Quaternion.LookRotation(_lookAt.position - tr.position);
            tr.rotation = rot;
        }
#endif

        public void Follow()
        {
            Stop();
            RotateWhenFollow = true;
            _moving = StartCoroutine(Following(FollowTarget));
        }

        public void FollowNoRot()
        {
            RotateWhenFollow = false;
        }
        
        public void Transition(Transform point, Transform lookPoint)
        {
            Stop();
            FollowTarget = point;
            LookAtTarget = lookPoint;
            _moving = StartCoroutine(TransitionToNewPoint(point, lookPoint));
        }
        
        public void Stop()
        {
            if(_moving != null)
                StopCoroutine(_moving);
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
                if (RotateWhenFollow)
                {
                    var rot = Quaternion.LookRotation(_lookAt.position - tr.position);
                    tr.rotation = rot;             
                }
                yield return null;
            }   
        }

        private IEnumerator TransitionToNewPoint(Transform point, Transform lookPoint)
        {
            var elapsed = 0f;
            var time = _transitionTime;
            var t = 0f;
            var tr = transform;
            var startPos = tr.position;
            var rot1 = tr.rotation;
            while (elapsed <= time)
            {
                tr.position = Vector3.Lerp(startPos, point.position, t);
                var rot2 = Quaternion.LookRotation(lookPoint.position - point.position);
                // var rot2 = point.rotation;
                tr.rotation =Quaternion.Lerp(rot1, rot2, t);
                t = elapsed / time;
                elapsed += Time.deltaTime * _transitionCurve.Evaluate(t);
                yield return null;
            }
            yield return Following(point);
        }

    }
}