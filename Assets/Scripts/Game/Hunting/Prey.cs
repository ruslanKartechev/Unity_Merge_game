using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class Prey : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;

        [SerializeField] private ParticleSystem _preyParticles;
        [SerializeField] private PreySettings _settings;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [Space(10)]
        [SerializeField] private List<PreyActionListener> _listeners;
        [SerializeField] private List<PreySurprisedListener> _surprisedListeners;
        private IPreyHealth _health;
        
        
        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.OnDead += OnDead;
            foreach (var listener in _listeners)
                listener.OnStarted();
        }

        public float GetReward() => _settings.Reward;
        
        public void SurpriseToAttack()
        {
            foreach (var surprisedListener in _surprisedListeners)
                surprisedListener.OnSurprised();
        }

        public ICamFollowTarget CamTarget => _camFollowTarget;

        public void IdleState()
        {
            // _preyAnimator.RandomIdle();
        }
        
        public void RunState()
        {
            foreach (var listener in _listeners)
                listener.OnBeganRun();
        }

        private void OnDead()
        {
            transform.SetParent(null);
            _health.Hide();
            StopParticles();
            foreach (var listener in _listeners)
                listener.OnDead();
            OnKilled?.Invoke(this);
        }
        
        private void StopParticles()
        {
            _preyParticles.Stop();
        }
        
    }
}