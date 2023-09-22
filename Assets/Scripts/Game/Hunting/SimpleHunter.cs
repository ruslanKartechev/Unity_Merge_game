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

        [SerializeField] private Vector2 _aimInflectionOffset;
        [SerializeField] private float _aimInflectionOffsetVisual;
        
        [SerializeField] private HunterAnimator _hunterAnimator;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _movable;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [Space(10)]
        [SerializeField] private HunterMouth _mouth;
        [SerializeField] private HunterMouthCollider _mouthCollider;
        [SerializeField] private RagdollBodyPusher _ragdollPusher;
        private IHunterSettings _settings;
        private IPreyPack _preyPack;
        private Coroutine _moving;
        private CamFollower _camFollower;
        
        
        public void Init(IHunterSettings settings, CamFollower camFollower)
        {
            _settings = settings;
            _positionAdjuster.enabled = true;
            _camFollower = camFollower;
            _mouthCollider.Activate(false);
        }
        
        public void SetPrey(IPreyPack preyPack) => _preyPack = preyPack;

        public float UpOffset() => _positionAdjuster.Offset;

        public Vector2 AimInflectionUpLimits() => _aimInflectionOffset;
        public float AimInflectionOffsetVisual() => _aimInflectionOffsetVisual;
        

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public Transform GetTransform() => transform;
        
        public void Run()
        {
            // Debug.Log("Run");
            _hunterAnimator.Run();
        }

        public void Idle()
        {
            // Debug.Log("idle");
            _hunterAnimator.Idle();
        }
        
        
        
        public void Jump(AimPath path)
        {
            _hunterAnimator.Jump();
            _movable.SetParent(null);
            StopJump();
            _moving = StartCoroutine(Jumping(path));
            _camFollower.MoveToTarget(_camFollowTarget, path.end, CamToPreyTime);
            _mouthCollider.Callback = Bite;
            _mouthCollider.Activate(true);
        }
        
        public void Celebrate()
        {
            _hunterAnimator.Idle();
        }

        public void RotateTo(Vector3 point)
        {
            transform.rotation = Quaternion.LookRotation(point - transform.position);
        }

        private void StopJump()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
        private IEnumerator Jumping(AimPath path)
        {
            var time = ((path.end - path.inflection).magnitude + (path.inflection - path.start).magnitude) / _settings.JumpSpeed;
            var elapsed = 0f;
            var rotLerpSpeed = .3f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, rotLerpSpeed);
                _movable.position = pos;    
                elapsed += Time.deltaTime;
                yield return null;
            }
            _mouthCollider.Activate(false);
            Ragdoll();
            _ragdollPusher.Push((path.end - path.start).normalized);
            yield return new WaitForSeconds(AfterAttackDelay);
            OnDead?.Invoke(this);
        }

        private void CallDelayedDead()
        {
            StopJump();
            _moving = StartCoroutine(DelayedDeadCall());
        }   

        private IEnumerator DelayedDeadCall()
        {
            yield return new WaitForSeconds(AfterAttackDelay);   
            OnDead?.Invoke(this);
        }

        private void Bite(Collider collider)
        {
            StopJump();
            var target = collider.GetComponent<IBiteTarget>();
            if (target == null)
            {
                _mouthCollider.Activate(true);
                return;
            }
            _mouthCollider.Activate(false);
            Ragdoll();
            var refPoint = target.GetClosestBitePosition(_mouthCollider.transform.position);
            _mouth.BiteTo(target.GetBiteParent(), refPoint);   
            target.Damage(new DamageArgs(_settings.Damage, refPoint.position));
            CallDelayedDead();
        }

        private void Ragdoll()
        {
            _animator.enabled = false;
            _ragdoll.Activate();
        }


    }
    

}