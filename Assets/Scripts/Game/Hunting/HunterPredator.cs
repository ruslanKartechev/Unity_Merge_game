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
    public class HunterPredator : MonoBehaviour, IHunter
    {
        public event Action<IHunter> OnDead;

        [Header("Config")] 
        [SerializeField] private HunterAimSettings _hunterAim;
        [SerializeField] private HuntersConfig _config;
        [Header("SlowMotion")]
        [SerializeField] private SlowMotionEffectSO _slowMotionEffect;
        [Header("Listeners")]
        [SerializeField] private List<HunterListener> _listeners;
        [Header("Camera")]
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [Header("Animator")]
        [SerializeField] private HunterAnimator _hunterAnimator;
        [Header("Ragdoll")]
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private RagdollBodyPusher _ragdollPusher;
        [Header("Biting")]
        [SerializeField] private HunterMouth _mouth;
        [SerializeField] private HunterMouthCollider _mouthCollider;
        [Header("Other")] 
        [SerializeField] private ItemDamageDisplay _damageDisplay;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [SerializeField] private Transform _movable;
        [SerializeField] private HunterMover _hunterMover;

        
        private IHunterSettings _settings;
        private Coroutine _moving;
        private CamFollower _camFollower;
        private TargetSeeker_Predator _predatorTargetSeeker;
        
        public CamFollower CamFollower
        {
            get => _camFollower;
            set => _camFollower = value;
        }

        private bool _isJumping;
        
        private Vector3 Position
        {
            get => _movable.position;
            set => _movable.position = value;
        }
        
        public void Init(IHunterSettings settings, MovementTracks track)
        {
            _settings = settings;
            _positionAdjuster.enabled = true;
            _mouthCollider.Activate(false);
            _damageDisplay.SetDamage(settings.Damage);
            _predatorTargetSeeker = new TargetSeeker_Predator(_mouthCollider.transform, _settings, _config.BiteMask);
            _hunterMover.SetSpline(track, track.main);
            _hunterMover.Speed = track.moveSpeed;
        }

        public IHunterSettings Settings => _settings;

        public void SetPrey(IPreyPack preyPack) {}

        public HunterAimSettings AimSettings => _hunterAim;

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public Transform GetTransform() => transform;
        
        public void Run()
        {
            if (_isJumping)
                return;
            _hunterAnimator.Run();
            _hunterMover.Move();
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
            _positionAdjuster.enabled = false;
            _hunterAnimator.Jump();
            _movable.SetParent(null);
            _hunterMover.StopMoving();
            StopJump();
            _moving = StartCoroutine(Jumping(path));
            _camFollower.MoveToTarget(_camFollowTarget, path.end);
            _mouthCollider.Activate(false);
            foreach (var listener in _listeners)
                listener.OnAttack();
            FlyParticles.Instance.Play();
            _slowMotionEffect.Begin();
            _damageDisplay.Hide();
        }
        
        public void Celebrate()
        {
            _hunterAnimator.Idle();
            _hunterMover.StopMoving();
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
            var slowMoOff = false;
            var time = ((path.end - path.inflection).magnitude + (path.inflection - path.start).magnitude) / _settings.JumpSpeed;
            var elapsed = 0f;
            var rotLerpSpeed = .3f;
            var t = 0f;
            var tMax = _config.JumpTMax;
            var unscaledElapsed = 0f;
            var slowMoTimeMax = _config.MaxSlowMoTime;
            
            while (t <= tMax)
            {
                t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, rotLerpSpeed);
                Position = pos;

                if (slowMoOff == false)
                {
                    if (unscaledElapsed >= slowMoTimeMax)
                    {
                        slowMoOff = true;
                        _slowMotionEffect.Stop();
                    }
                }
                
                if(CheckEnemy())
                    yield break;
                
                elapsed += Time.deltaTime;
                unscaledElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            HitGround();
            yield return new WaitForSeconds(_config.AfterAttackDelay);
            OnDead?.Invoke(this);
        }
        

        private void HitGround()
        {
            FlyParticles.Instance.Stop();
            _slowMotionEffect.Stop();
            foreach (var listener in _listeners)
                listener.OnFall();
            _mouthCollider.Activate(false);
            _hunterAnimator.Disable();
            _ragdoll.Activate();
            _ragdollPusher.Push(transform.forward);   
        }
        
        private void StopJumpAndEffects()
        {
            _slowMotionEffect.Stop();
            FlyParticles.Instance.Stop();
            StopJump();   
        }
        
        private void CallDelayedDead()
        {
            StopJump();
            _moving = StartCoroutine(DelayedDeadCall());
        }   

        private IEnumerator DelayedDeadCall()
        {
            yield return new WaitForSeconds(_config.AfterAttackDelay);   
            OnDead?.Invoke(this);
        }

        private bool CheckEnemy()
        {
            if(_predatorTargetSeeker.GetHit(transform, out var hit))
            {
                var target = TryGetTarget(hit.collider.gameObject);
                if(target == null || target.IsAlive() == false)
                    return false;
                BiteEnemy(target, hit.collider.transform, hit.point);
            }
            return false;
        }

        private IPredatorTarget TryGetTarget(GameObject go)
        {
            return go.GetComponentInParent<IPredatorTarget>();
        } 
        
        private void BiteEnemy(IPredatorTarget target, Transform enemy, Vector3 hitPoint)
        {
            foreach (var listener in _listeners)
                listener.OnBite();
            StopJumpAndEffects();
            _mouthCollider.Activate(false);
            _hunterAnimator.Disable();
            if (target.IsBiteable())
                Bite(target, enemy, hitPoint);
            else
                DamageOnly(target);
            CallDelayedDead();
        }

        private void Bite(IPredatorTarget target, Transform parent, Vector3 point)
        {
            target.Damage(new DamageArgs(_settings.Damage, point));
            _mouth.BiteTo( _movable, parent, null, point);   
            _ragdoll.Activate();
        }

        private void DamageOnly(IPredatorTarget target)
        {
            target.Damage(new DamageArgs(_settings.Damage, _mouthCollider.transform.position));
            _ragdoll.Activate();
            _ragdollPusher.Push(transform.forward);   
        }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damageDisplay == null)
            {
                _damageDisplay = GetComponentInChildren<ItemDamageDisplay>();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}