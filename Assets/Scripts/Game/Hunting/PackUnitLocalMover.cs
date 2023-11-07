using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class PackUnitLocalMover : MonoBehaviour
    {
        [SerializeField] private LocalRotator _rotator;
        [SerializeField] private Transform _movable;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotToDirTime;
        [SerializeField] private float _rotToIdentityTime;
        private Coroutine _moving;

        public void SetPoint(Transform point)
        {
            _targetPoint = point;
            #if UNITY_EDITOR
            if(Application.isPlaying == false)
                UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
        
#if UNITY_EDITOR
        [SerializeField] private bool _drawGizmo;
        [SerializeField] private Color _gizmoColor;

        private void OnDrawGizmos()
        {
            if (_drawGizmo == false)
                return;
            if (_movable == null || _targetPoint == null)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = _gizmoColor;
            Debug.DrawLine(_movable.position + Vector3.up * .2f,
                _targetPoint.position + Vector3.up * .2f);
            Gizmos.color = oldColor;
        }
#endif

        public void MoveToLocalPoint()
        {
            _moving = StartCoroutine(MovingToPoint());
        }

        public void RotateToPoint()
        {
            var rot = Quaternion.LookRotation(_targetPoint.position - _movable.position);
            _rotator.RotateTo(rot, _rotToDirTime);
        }

        public void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }

        public void StopRotating()
        {
            _rotator.Stop();   
        }
        
        private IEnumerator MovingToPoint()
        {
            var elapsed = 0f;
            var p1 = _movable.localPosition;
            var p2 = _targetPoint.localPosition;
            var time = (p2 - p1).magnitude / _moveSpeed;
            while (elapsed <= time)
            {
                _movable.localPosition = Vector3.Lerp(p1, p2, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _movable.localPosition = p2;
            _rotator.RotateTo(Quaternion.identity, _rotToIdentityTime);
        }
    }
}