﻿using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Ragdoll;
using Common.SlowMotion;
using Game.Hunting.HuntCamera;
using Game.Hunting.Hunters.Interfaces;
using Game.Hunting.Prey.Interfaces;
using Game.Merging.View;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.Hunting.Hunters
{
    public class HunterAir : MonoBehaviour, IHunter
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

        
        private IHunterSettings_Air _settings;
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
            set => _movable.position = value;
        }
        
        public void Init(string item_id, MovementTracks track)
        {
            _settings = GC.HunterSettingsProvider.GetSettingsAir(item_id);
            _positionAdjuster.enabled = true;
            _mouthCollider.Activate(false);
            _damageDisplay.SetDamage(_settings.Damage);
            _hunterMover.SetSpline(track, track.main);
            _hunterMover.Speed = track.moveSpeed;
        }

        public IHunterSettings Settings => _settings;

        public HunterAimSettings AimSettings => _hunterAim;

        public ICamFollowTarget CameraPoint => _camFollowTarget;

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
            var target = GetTarget(path);
            // Debug.Log($"[Bird] target null {target == null}");
            if (target != null)
            {
                // Debug.Log($"[Bird] Fly to Target");
                _moving = StartCoroutine(FlyingToTarget(target));
            }
            else
            {
                // Debug.Log($"[Bird] Fly to Empty");
                foreach (var listener in _listeners)
                    listener.OnAttack();
                _moving = StartCoroutine(FlyingToEmpty(path));                
            }
            _camFollower.MoveToTarget(_camFollowTarget, path.end);
            _mouthCollider.Activate(false);
  
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

        private IEnumerator FlyingToEmpty(AimPath path)
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
                _movable.rotation = Quaternion.Lerp(_movable.rotation, 
                    Quaternion.LookRotation(path.end - pos), 
                    rotLerpSpeed);
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
        
        private IEnumerator FlyingToTarget(IAirTarget target)
        {
            var slowMoOff = false;
            var flyToTr = target.GetFlyToTransform();
            var startPos = _movable.position;
            
            var time = (startPos - flyToTr.position).magnitude / _settings.JumpSpeed;
            var elapsed = 0f;
            var rotLerpSpeed = .3f;
            var t = 0f;
            var tMax = _config.JumpTMax;
            var unscaledElapsed = 0f;
            var slowMoTimeMax = _config.MaxSlowMoTime;
            while (t <= tMax)
            {
                t = elapsed / time;

                var endPos = flyToTr.position;
                var infPos = Vector3.Lerp(startPos, endPos, _hunterAim.ArcInflectionLerp);
                infPos += Vector3.up * _hunterAim.AimInflectionUpLimits.y;
                var pos = Bezier.GetPosition(startPos, infPos, endPos, t);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, 
                    Quaternion.LookRotation(pos - Position), 
                    rotLerpSpeed);
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
            Position = flyToTr.position;
            HitTarget(target);
        }

        private void HitTarget(IAirTarget target)
        {
            // Debug.Log($"[Bird] Hit target");
            if (target.CanGrabToAir())
            {
                _movable.SetParent(target.MoverParent());
                target.GrabTo(_movable);
                target.Damage(new DamageArgs(_settings.Damage, _movable.position));
                if (target.IsAlive())
                    LiftAndDropAlive(target);
                else
                    LiftAndDropDead(target);
            }
            else
            { 
                // Debug.Log($"[Bird] Cannot bite, Flying away");
                target.Damage(new DamageArgs(_settings.Damage, _movable.position));
                FlyAwayUp(HideSelf);
                RaiseOnDead();
            }
        }

        private void RaiseOnDead()
        {
            OnDead?.Invoke(this);
        }

        private void LiftAndDropDead(IAirTarget target)
        {
            // Debug.Log($"[Bird] Lift and kill");
            _hunterAnimator.Run();
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(LiftingEnemyUp(() =>
            {
                RaiseOnDead();
                FlyAwayUp(() =>
                {
                    DropTargetAndHide(target);
                });
            }));
        }

        private void LiftAndDropAlive(IAirTarget target)
        {
            // Debug.Log($"[Bird] Lift enemy up");
            _hunterAnimator.Run();
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(LiftingEnemyUp(() =>
            {
                RaiseOnDead();
                target.DropAlive();
                FlyAwayUp(HideSelf);
            }));
        }
        
        private void DropTargetAndHide(IAirTarget target)
        {
            target.DropDead();
            HideSelf();
        }
        
        private void HideSelf()
        {
            gameObject.SetActive(false);
        }
        
        private void FlyAwayUp(Action onEnd)
        {
            _hunterAnimator.Run();
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(FlyingAway(onEnd));
        }
        
        private IEnumerator LiftingEnemyUp(Action onEnd)
        {
            var elapsed = 0f;
            var time = _settings.LiftUpDuration;
            var upOffset = _settings.LiftUpHeight;
            var localPos = _movable.localPosition;
            var yStart = localPos.y;
            var yEnd = yStart + upOffset;
            while (elapsed <= time)
            {
                _movable.localPosition = new Vector3(localPos.x, Mathf.Lerp(yStart, yEnd, elapsed / time), localPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            onEnd.Invoke();
        }

        private IEnumerator FlyingAway(Action onEnd)
        {
            var elapsed = 0f;
            var startPos = _movable.position;
            var forwDir = new Vector3( _movable.forward.x, 0f,  _movable.forward.z);
            var offset = _settings.FlyAwayOffset;
            var endPos = startPos + forwDir * offset.z + Vector3.up * offset.y;
            var time = _settings.FlyAwayDuration;
            
            while (elapsed <= time)
            {
                _movable.position = Vector3.Lerp(startPos, endPos, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            onEnd.Invoke();
        }
        
        private IAirTarget GetTarget(AimPath path)
        {
            var ray = new Ray(path.start, path.end - path.start);
            if (Physics.SphereCast(ray, _settings.Radius, out var hit, 100, _config.BiteMask))
            {
                var target = hit.collider.GetComponentInParent<IAirTarget>();
                return target;
            }
            return null;
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
        
        
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damageDisplay == null)
            {
                _damageDisplay = GetComponentInChildren<ItemDamageDisplay>();
                UnityEditor.EditorUtility.SetDirty(this);
            }

            var hunterListeners = GetComponentsInChildren<HunterListener>();
            foreach (var listener in hunterListeners)
            {
                if(_listeners.Contains(listener) == false)
                    _listeners.Add(listener);
            }
            if(_camFollowTarget == null)
                _camFollowTarget = GetComponentInChildren<CamFollowTarget>();
            if(_hunterAnimator == null)
                _hunterAnimator = GetComponentInChildren<HunterAnimator>();
            if(_ragdoll == null)
                _ragdoll = GetComponentInChildren<IRagdoll>();
            if(_ragdollPusher == null)
                _ragdollPusher = GetComponentInChildren<RagdollBodyPusher>();
            if(_mouth == null)
                _mouth = GetComponentInChildren<HunterMouth>();
            if(_mouthCollider == null)
                _mouthCollider = GetComponentInChildren<HunterMouthCollider>();
            if(_hunterMover == null)
                _hunterMover = GetComponentInChildren<HunterMover>();
            if(_positionAdjuster == null)
                _positionAdjuster = GetComponentInChildren<OnTerrainPositionAdjuster>();
            if(_movable == null)
                _movable = transform;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}