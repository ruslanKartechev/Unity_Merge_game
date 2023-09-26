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

        public int CameraFlyDir { get; set; }

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
                var oldPos = _movable.position;
                var lerpPos =  Vector3.Lerp(_movable.position, targetPos, .02f);
                _movable.position = lerpPos;
                var rot = Quaternion.LookRotation(_moveTarget.GetPosition() - lerpPos);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rot, .02f);
                yield return null;
            }
        }
        
        
        private IEnumerator MoveToAndFollow(ICamFollowTarget target, Vector3 targetPoint)
        {
            var settings = target.CameraSettings ?? _settings;
            var elapsed = 0f;
            var offset = _movable.position - target.GetPosition();
            var forward = targetPoint - _movable.position;
            forward.y = 0;
            forward.Normalize();
            var rightDir = Vector3.Cross(-forward, Vector3.up) * CameraFlyDir;
                        // * (UnityEngine.Random.Range(0f, 1f) >= 0.5f ? -1f : 1f); // if we want to randomize
            var y = _movable.position.y;
            var maxOffset = _settings.followUpOffsetMax;
            var time = _settings.followUpOffsetSetTime;
            
            while (true)
            {
                var t = elapsed / time;
                var targetPos = target.GetPosition();
                var nextPosition = targetPos + offset;
                if(settings.maintainY)
                    nextPosition.y = y + Mathf.Lerp(0, maxOffset, t);
                else
                    nextPosition.y = nextPosition.y + Mathf.Lerp(0, maxOffset, t);

                var sideOffset = Mathf.Lerp(_settings.sideOffsetLimits.x, 
                    _settings.sideOffsetLimits.y, 
                    elapsed / _settings.SideOffsetMoveTime);
                var forwardOffset = Mathf.Lerp(_settings.forwardOffsetLimits.x, 
                    _settings.forwardOffsetLimits.y, 
                    elapsed / _settings.forwardOffsetMoveTime);
                
                // Debug.Log($"side offset: {sideOffset}, RIGHT DIR: {rightDir}");
                var pos = Vector3.Lerp(_movable.position, nextPosition, _settings.lerpFollowSpeed)
                          + rightDir * sideOffset
                          + forward * forwardOffset;
                _movable.position = pos;
                
                var rotation = Quaternion.LookRotation(target.GetLookAtPosition() - pos);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rotation, _settings.lerpRotSpeed);
                
                elapsed += Time.unscaledDeltaTime;
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