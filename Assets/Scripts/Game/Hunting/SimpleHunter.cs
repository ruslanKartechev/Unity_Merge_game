using System;
using System.Collections;
using Common;
using Common.Ragdoll;
using Game.Hunting.HuntCamera;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class SimpleHunter : MonoBehaviour, IHunter
    {
        public event Action<IHunter> OnDead;
        
        private const float AfterAttackDelay = 1f;
        private const float CamToPreyTime = 1f;

        [SerializeField] private HunterAnimator _hunterAnimator;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _movable;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private HunterMouth _mouthPrefab;
        [SerializeField] private Transform _raycastDir;
        [SerializeField] private Rigidbody _headRb;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [SerializeField] private float _sphereCastRad = 0.3f;
        private IHunterSettings _settings;
        private IPreyPack _preyPack;
        private Coroutine _moving;
        private CamFollower _camFollower;
        
        
        public void Init(IHunterSettings settings, CamFollower camFollower)
        {
            _settings = settings;
            _positionAdjuster.enabled = true;
            _camFollower = camFollower;
        }
        
        public void SetPrey(IPreyPack preyPack) => _preyPack = preyPack;

        public void Run()
        {
            _hunterAnimator.Run();
        }

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public Transform GetTransform() => transform;
        
        
        public void Jump(AimPath path)
        {
            _hunterAnimator.Jump();
            _movable.SetParent(null);
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(Jumping(path));
            MoveCameraToClosestPrey(path.end);
        }

        private void MoveCameraToClosestPrey(Vector3 position)
        {
            _camFollower.MoveToTarget(_preyPack.AttackCamTarget, position, CamToPreyTime);
        }

        public void Celebrate()
        {
            _hunterAnimator.Idle();
        }

        private IEnumerator Jumping(AimPath path)
        {
            var time = ((path.end - path.inflection).magnitude + (path.inflection - path.start).magnitude) 
                            / _settings.JumpSpeed;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, 0.1f);
                #if UNITY_EDITOR
                Debug.DrawLine(_movable.position, pos, Color.green, 3f);
                #endif
                _movable.position = pos;    
                elapsed += Time.deltaTime;
                if (TryBite())
                {
                    yield return new WaitForSeconds(AfterAttackDelay);   
                    OnDead?.Invoke(this);
                    yield break;
                }
                yield return null;
            }
            _animator.enabled = false;
            _ragdoll.Activate();
            yield return new WaitForSeconds(AfterAttackDelay);
            OnDead?.Invoke(this);
        }

        private bool TryBite()
        {
            if (Physics.SphereCast(_raycastDir.position, _sphereCastRad, _raycastDir.forward, 
                    out var hit, _settings.BiteDistance, _settings.BiteMask))
            {
                var target = hit.collider.gameObject.GetComponent<IBiteTarget>();
                target.Damage(new DamageArgs(_settings.Damage, hit.point));
                _animator.enabled = false;
                _ragdoll.Activate();
                var mouth = Instantiate(_mouthPrefab);
                mouth.transform.position = hit.point - (hit.point - _raycastDir.position).normalized * _settings.BiteOffset;
                mouth.BiteTo(target.GetBiteBone(), _headRb);   
                
                return true;
            }
            return false;
        }
        
    }
}