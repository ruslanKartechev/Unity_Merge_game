using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Ragdoll;
using Common.SlowMotion;
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
        [Space(10)] 
        [SerializeField] private SlowMotionEffectSO _slowMotionEffect;
        [SerializeField] private List<HunterListener> _listeners;
        [Space(10)]
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
        
        public CamFollower CamFollower
        {
            get => _camFollower;
            set => _camFollower = value;
        }

        private bool _isJumping;
        private Vector3 Position
        {
            get => _movable.position;
            set
            {
                _movable.position = value;
                // if(_debugPos)
                    // Debug.Log($"Pos: {value}");
            }
        }
        
        public void Init(IHunterSettings settings)
        {
            _settings = settings;
            _positionAdjuster.enabled = true;
            _mouthCollider.Activate(false);
        }
        
        public void SetPrey(IPreyPack preyPack) => _preyPack = preyPack;

        public Vector2 AimInflectionUpLimits() => _aimInflectionOffset;
        public float AimInflectionOffsetVisual() => _aimInflectionOffsetVisual;
        

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public Transform GetTransform() => transform;
        
        public void Run()
        {
            if (_isJumping)
                return;
            _hunterAnimator.Run();
        }

        public void Idle()
        {
            if (_isJumping)
                return;
            _hunterAnimator.Idle();
        }
        
        public void Jump(AimPath path)
        {
            _isJumping = true;
            _hunterAnimator.Jump();
            _movable.SetParent(null);
            StopJump();
            _moving = StartCoroutine(Jumping(path));
            _camFollower.MoveToTarget(_camFollowTarget, path.end, CamToPreyTime);
            _mouthCollider.Callback = Bite;
            _mouthCollider.Activate(true);
            _positionAdjuster.enabled = false;
            foreach (var listener in _listeners)
                listener.OnAttack();
            FlyParticles.Instance.Play();
            _slowMotionEffect.Begin();
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
            var t = 0f;
            var t_max = 0.9f;
            while (t <= t_max)
            {
                t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, rotLerpSpeed);
                Position = pos;    
                elapsed += Time.deltaTime;
                yield return null;
            }
            FlyParticles.Instance.Stop();
            _slowMotionEffect.Stop();
            foreach (var listener in _listeners)
                listener.OnFall();
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
            var target = collider.GetComponent<IBiteTarget>();
            if (target == null)
            {
                _mouthCollider.Activate(true);
                return;
            }
            _slowMotionEffect.Stop();
            FlyParticles.Instance.Stop();
            StopJump();
            foreach (var listener in _listeners)
                listener.OnBite();
            var contactPoint = collider.ClosestPoint(transform.position);
            StartCoroutine(Biting(target, contactPoint));
        }

        private IEnumerator Biting(IBiteTarget target, Vector3 contactPoint)
        {
            _mouthCollider.Activate(false);
            _animator.enabled = false;

            var refPoint = target.GetClosestBitePosition(transform.position + Vector3.up);
            target.Damage(new DamageArgs(_settings.Damage, refPoint.position));
            
            // yield return new WaitForFixedUpdate();
            _mouth.BiteTo( _movable, target.GetBiteParent(), refPoint, contactPoint);   
            // yield return new WaitForFixedUpdate();
            _ragdoll.Activate();
            yield return new WaitForFixedUpdate();
            CallDelayedDead();
        }
        
        private void Ragdoll()
        {
            _animator.enabled = false;
            _ragdoll.Activate();
        }

    }
}