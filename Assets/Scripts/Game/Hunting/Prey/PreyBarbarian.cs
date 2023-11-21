﻿using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBarbarian : MonoBehaviour, IPrey, IPredatorTarget, IFishTarget, IAirTarget
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private bool _canBiteTo = true;
        [SerializeField] private bool _canGrabToAir = true;
        [SerializeField] private Transform _airTarget;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [Space(10)]
        [SerializeField] private PreySettings _settings;
        [SerializeField] private BarbarianInAirBehaviour _inAirBehaviour;
        [SerializeField] private MonoBehaviour _airDropDeadBehaviour;
        [SerializeField] private MonoBehaviour _airDropAliveBehaviour;
        [Header("Chosen Behvaiours")]
        [SerializeField] private MonoBehaviour _idleBehaviour;
        [SerializeField] private MonoBehaviour _surprisedBehaviour;
        [SerializeField] private MonoBehaviour _runBehaviour;
        [SerializeField] private MonoBehaviour _deadBehaviour;
        
        
        private PreyHealth _health;
        private IPreyBehaviour _currentBehaviour;
        private bool _isGrabbedToAir;
        private bool _isAvailableTarget = true;
        
        public PreySettings PreySettings
        {
            get => _settings;
            set => _settings = value;
        }

        public ICamFollowTarget CamTarget => _camFollowTarget;
        public bool IsAvailableTarget => _isAvailableTarget;

        public float GetReward() => _settings.Reward;

        
        public void Init()
        {
            _health = gameObject.GetComponent<PreyHealth>();
            _health.Init(_settings.Health);
            _currentBehaviour = (IPreyBehaviour)_idleBehaviour;
            _currentBehaviour.Begin();
        }
        
        public void OnPackAttacked()
        {
            if (!_health.IsAlive() || _isGrabbedToAir)
                return;
            // CLog.LogRed($"Surprised {gameObject.name}");
            BeginBehaviour(_surprisedBehaviour);
        }

        public void OnPackRun()
        {
            if (!_health.IsAlive() || _isGrabbedToAir)
                return;
            BeginBehaviour(_runBehaviour);
        }

        public void Damage(DamageArgs damageArgs)
        {
            if (!_health.IsAlive())
                return;
            _health.Damage(damageArgs);
            if (_isGrabbedToAir)
            {
                // Debug.Log($"Damaged while in air");
                return;
            }
            if (!_health.IsAlive())
                Die();
        }

        public bool IsAlive() => _health.IsAlive();

        // MAKE IT APPROPRIATE !
        public Vector3 GetShootAtPosition()
        {
            return transform.position + Vector3.up;
        }

        public bool CanBindTo() => _canBiteTo;
        
        public bool CanGrabToAir() => _canGrabToAir;
        
        public Transform GetFlyToTransform() => _airTarget;

        public Transform MoverParent() => transform.parent;

        public void GrabTo(Transform tr, DamageArgs damage)
        {
            // CLog.LogGreen($"Grabbed {gameObject.name}");
            _isAvailableTarget = false;
            _health.Hide();
            _isGrabbedToAir = true;
            _currentBehaviour?.Stop();
            _inAirBehaviour.OnGrabbed(tr);
            Damage(damage);
            if(IsAlive() == false)
                RaiseKilled();
        }

        public void DropAlive()
        {
            _isGrabbedToAir = false;
            _isAvailableTarget = true;
            BeginBehaviour(_airDropAliveBehaviour);
        }

        public void DropDead()
        {
            _isGrabbedToAir = false;
            _isAvailableTarget = false;
            BeginBehaviour(_airDropDeadBehaviour);
            RaiseKilled();
        }
        
        private void BeginBehaviour(MonoBehaviour script)
        {
            _currentBehaviour.Stop();
            _currentBehaviour = (IPreyBehaviour)script;
            _currentBehaviour.Begin();
        }
        
        private void Die()
        {
            _health.Hide();
            BeginBehaviour(_deadBehaviour);
            RaiseKilled();
        }
        
        private void RaiseKilled()
        {
            OnKilled?.Invoke(this);
        }
        
        
        
        #region Editor
#if UNITY_EDITOR
        [Space(20)]
        [SerializeField] private List<MonoBehaviour> _idleBehaviourOptions;
        [SerializeField] private List<MonoBehaviour> _surprisedBehaviourOptions;
        [SerializeField] private List<MonoBehaviour> _runBehaviourOptions;
        [SerializeField] private List<MonoBehaviour> _deadBehaviourOptions;
        private int _idleIndex;
        private int _surprisedIndex;
        private int _runIndex;
        private int _deadIndex;
        
        public void SwitchIdle(bool next)
        {
            _idleBehaviour = GetSwitchBehaviour(_idleBehaviourOptions, next, ref _idleIndex);
            Dirty();
        }
        public void SwitchSurprised(bool next)
        {
            _surprisedBehaviour = GetSwitchBehaviour(_surprisedBehaviourOptions, next, ref _idleIndex);
            Dirty();
        }
        
        public void SwitchRun(bool next)
        {
            _runBehaviour = GetSwitchBehaviour(_runBehaviourOptions, next, ref _idleIndex);
            Dirty();
        }
        
        public void SwitchDead(bool next)
        {
            _deadBehaviour = GetSwitchBehaviour(_deadBehaviourOptions, next, ref _idleIndex);
            Dirty();
        }
        
        public void Dirty()
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        private MonoBehaviour GetSwitchBehaviour(IList<MonoBehaviour> list, bool next, ref int index)
        {
            if(next)
                index++;
            else
                index--;
            index = Mathf.Clamp(index, 0, list.Count - 1);
            if (list.Count == 0)
                return null;
            return list[index];
        }
#endif
        #endregion
    }
}

