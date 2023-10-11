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
    public class HunterFish : MonoBehaviour, IHunter
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
        [SerializeField] private SmallFishTank _fishTank;
        [Header("Biting")]
        [SerializeField] private HunterMouthCollider _mouthCollider;
        [Header("Other")]
        [SerializeField] private ItemDamageDisplay _damageDisplay;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [SerializeField] private Transform _movable;

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
        
        public HunterAimSettings AimSettings => _hunterAim;
        

        public void Init(IHunterSettings settings)
        {
            _settings = settings;
            _positionAdjuster.enabled = true;
            _mouthCollider.Activate(false);
            _damageDisplay.SetDamage(settings.Damage);
        }
        
        public void SetPrey(IPreyPack preyPack) => _preyPack = preyPack;

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public Transform GetTransform() => transform;
        
        public IHunterSettings Settings => _settings;

        public void Run()
        { }

        public void Idle()
        {             
            _fishTank.Idle();
        }
        
        public void Jump(AimPath path)
        {
            _isJumping = true;
            _movable.SetParent(null);
            StopJump();
            _moving = StartCoroutine(Jumping(path));
            _camFollower.MoveToTarget(_camFollowTarget, path.end);
            _mouthCollider.Callback = ApplyDamage;
            _mouthCollider.Activate(true);
            _positionAdjuster.enabled = false;
            foreach (var listener in _listeners)
                listener.OnAttack();
            FlyParticles.Instance.Play();
            _slowMotionEffect.Begin();
            _fishTank.AlignToAttack();
            _damageDisplay.Hide();
        }
        
        public void Celebrate()
        { }

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
            _mouthCollider.Activate(false);
            foreach (var listener in _listeners)
                listener.OnFall();    
            BreakPushFish();
        }
        
        private void BreakPushFish()
        {
            _fishTank.PushRandomDir();       
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

        private void ApplyDamage(Collider collider)
        {
            var target = collider.GetComponent<IPredatorTarget>();
            if (target == null)
                return;
            _slowMotionEffect.Stop();
            target.Damage(new DamageArgs(_settings.Damage, collider.ClosestPoint(transform.position)));
            FlyParticles.Instance.Stop();
            StopJump();
            foreach (var listener in _listeners)
                listener.OnBite();
            BreakPushFish();
            CallDelayedDead();
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