﻿using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.SlowMotion;
using Game.Hunting.HuntCamera;
using Game.Hunting.Hunters.Interfaces;
using Game.Merging.View;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.Hunting.Hunters
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
        [SerializeField] private HunterMover _hunterMover;
        [SerializeField] private FishTrackModeSetter _trackModeSetter;
        [SerializeField] private FishParticlesManager _fishParticlesManager;

        private IHunterSettings_Water _settings;
        private Coroutine _moving;
        private TargetSeeker_Fish _targetSeeker;
        private bool _isDead;
        
        public IJumpCamera CamFollower { get; set; }

        private Vector3 Position
        {
            get => _movable.position;
            set => _movable.position = value;
        }
        
        public HunterAimSettings AimSettings => _hunterAim;
        

        public void Init(string item_id, MovementTracks track)
        {
            _settings = GC.HunterSettingsProvider.GetSettingsWater(item_id);
            Init(track);
        }
        
#if UNITY_EDITOR
        [Header("Debug SETTINGS")] 
        public HunterSettingsWater debugSettings;
        public void InitSelf(MovementTracks track)
        {
            _settings = debugSettings;   
            Init(track);
        }
#endif

        private void Init(MovementTracks track)
        {
            _positionAdjuster.enabled = true;
            _mouthCollider.Activate(false);
            _damageDisplay.SetDamage(_settings.Damage);
            _targetSeeker = new TargetSeeker_Fish(_mouthCollider.transform, _settings, _config.FishDamageMask, _fishTank);
            _mouthCollider.Activate(false);
            _hunterMover.SetSpline(track, track.water != null ? track.water : track.main);
            _hunterMover.Speed = track.moveSpeed;
            if (track.water != null)
            {
                _fishParticlesManager.IdleOnWater();
                _trackModeSetter.SetWater();
            }
            else
            {
                _fishParticlesManager.IdleOnLand();
                _trackModeSetter.SetLand();
            }     
        }

        public ICamFollowTarget CameraPoint => _camFollowTarget;

        public Transform GetTransform() => transform;
        
        public IHunterSettings Settings => _settings;

        public void Run()
        {
            if (_isDead)
                return;
            _hunterMover.Move();
        }

        public void Idle()
        {             
            _fishTank.Idle();
        }
        
        public void Jump(AimPath path)
        {
            _isDead = true;
            _movable.SetParent(null);
            _fishParticlesManager.JumpAttack();
            StopJump();
            _moving = StartCoroutine(Jumping(path));
            CamFollower.FollowInJump(_camFollowTarget, path.end);
            _positionAdjuster.enabled = false;
            _hunterMover.StopMoving();
            foreach (var listener in _listeners)
                listener.OnAttack();
            FlyParticles.Instance.Play();
            // _slowMotionEffect.Begin();
            _fishTank.AlignToAttack();
            _damageDisplay.Hide();
        }

        public void Celebrate()
        {
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
            var time = ((path.end - path.inflection).magnitude + (path.inflection - path.start).magnitude) / _settings.JumpSpeed;
            var elapsed = 0f;
            var rotLerpSpeed = .3f;
            var t = 0f;
            var tMax = _config.JumpTMax;
            while (t <= tMax)
            {
                t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, rotLerpSpeed);
                Position = pos;
                elapsed += Time.deltaTime;
                yield return null;
            }
            HitGround();
            yield return new WaitForSeconds(_config.AfterAttackDelay);
            RaiseOnDead();
        }

        private void HitGround()
        {
            FlyParticles.Instance.Stop();
            // _slowMotionEffect.Stop();
            _targetSeeker.Attack();
            _fishParticlesManager.HitEnemy();
            foreach (var listener in _listeners)
            {
                listener.OnFall();    
                listener.OnHitEnemy();
            }
        }
        
        private void RaiseOnDead()
        {
            OnDead?.Invoke(this);
        }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damageDisplay == null)
            {
                _damageDisplay = GetComponentInChildren<ItemDamageDisplay>();
                UnityEditor.EditorUtility.SetDirty(this);
            }
            if(_trackModeSetter == null)
                _trackModeSetter = GetComponentInChildren<FishTrackModeSetter>();
            if(_hunterMover == null)
                _hunterMover = GetComponentInChildren<HunterMover>();
            if(_damageDisplay == null)
                _damageDisplay = GetComponentInChildren<ItemDamageDisplay>();
        }
#endif
    }
}