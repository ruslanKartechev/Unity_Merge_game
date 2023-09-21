using System.Collections;
using UnityEngine;

namespace Game.Hunting.HuntCamera
{

    public class CamFollower : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        [SerializeField] private CameraSettings _settings;
        [SerializeField] private AnimationCurve _targetChangeCurve;
        private ICamFollowTarget _moveTarget;
        private ICamFollowTarget _lookTarget;
        private Coroutine _moving;


        public void MoveToTarget(ICamFollowTarget target, Vector3 position, float time)
        {
            Stop();
            _moving = StartCoroutine(MoveToAndFollow(target, position));
        }
        
        public void SetSingleTarget(ICamFollowTarget target)
        {
            _moveTarget = target;
            Stop();
            _moving = StartCoroutine(SingleTargetFollowing());
        }
        
        public void SetTargets(ICamFollowTarget moveTarget, ICamFollowTarget lookTarget, bool warpTo = false)
        {
            _moveTarget = moveTarget;
            _lookTarget = lookTarget;
            Stop();
            if (!warpTo)
            {
                _moving = StartCoroutine(Transitioning());
                return;
            }
            _moving = StartCoroutine(Moving());
        }

        private void Stop()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
        private IEnumerator Transitioning()
        {
            var time = (transform.position - _moveTarget.GetPosition()).magnitude / _settings.moveToTargetSpeed;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                SetPositionAndRot(t);
                elapsed += Time.deltaTime * _targetChangeCurve.Evaluate(t);
                yield return null;
            }
            _moving = StartCoroutine(Moving());
        }
        
        private IEnumerator Moving()
        {
            while (true)
            {
                SetPositionAndRot();
                yield return null;
            }
        }

        private IEnumerator SingleTargetFollowing()
        {
            var localOffset = _movable.position - _moveTarget.GetPosition();
            while (true)
            {
                var targetPos = _moveTarget.GetPosition() + localOffset;
                _movable.position = Vector3.Lerp(_movable.position, targetPos + localOffset, .1f);
                var rot = Quaternion.LookRotation(targetPos - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rot, .1f);
                yield return null;
            }
        }
        
        
        private IEnumerator MoveToAndFollow(ICamFollowTarget target, Vector3 targetPoint)
        {
            var elapsed = 0f;
            var startOffset = target.WorldToLocal(_movable.position);
            var offset = _movable.position - target.GetPosition();
            
            var pp = target.GetPosition();
            var y = _movable.position.y;
            var maxOffset = _settings.followUpOffsetMax;
            var time = _settings.followUpOffsetSetTime;
            while (true)
            {
                var t = elapsed / time;
                pp = target.GetPosition() + offset;
                pp.y = y + Mathf.Lerp(0, maxOffset, t);
                _movable.position = Vector3.Lerp(_movable.position, pp, _settings.lerpFollowSpeed);
                
                var rotation = Quaternion.LookRotation((target.GetPosition() - _movable.position));
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rotation, _settings.lerpRotSpeed);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
        }        
        
        
        private IEnumerator MovingToTarget(ICamFollowTarget target, Vector3 position, float time)
        {
            var startY = _movable.position.y;
            var localTargetPos = position - target.GetPosition();
            var offset = _movable.position - position;
            var distance = new Vector2(offset.x, offset.z).magnitude;
            offset.Normalize();
            
            var distanceFactor = 0.5f;
            var minDistance = distance * distanceFactor;
            
            var elapsed = 0f;
            while (true)
            {
                var t = elapsed / time;
                var offsetLength = Mathf.Lerp(distance, minDistance, t);
                var targetPos = localTargetPos + target.GetPosition();
                var pos = targetPos + offset * offsetLength;
                pos.y = startY;
                _movable.position = pos;
                _movable.rotation = Quaternion.LookRotation(targetPos - pos);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        private void SetPositionAndRot(float t)
        {
            var moveToPos = GetPositionRelativeToLook();
            _movable.position = Vector3.Lerp(_movable.position, moveToPos, t);
            _movable.rotation = Quaternion.Lerp(_movable.rotation, Quaternion.LookRotation(_lookTarget.GetPosition() - moveToPos), t);
        }

        private void SetPositionAndRot()
        {
            var moveToPos = GetPositionRelativeToLook();
            _movable.SetPositionAndRotation(moveToPos, Quaternion.LookRotation(_lookTarget.GetPosition() - moveToPos));      
        }

        private Vector3 GetPositionRelativeToLook()
        {
            var moveToPos = _moveTarget.GetPosition();
            var lookVector = _lookTarget.GetPosition() - moveToPos;
            var offset = _moveTarget.GetOffset();
            return moveToPos - lookVector.normalized * offset.z + Vector3.up * offset.y;
        }
    }
}