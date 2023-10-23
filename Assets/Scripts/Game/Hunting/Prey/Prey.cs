using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class Prey : MonoBehaviour, IPrey, IHealthListener
    {
        public event Action<IPrey> OnKilled;

        [SerializeField] private PreySettings _settings;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [Space(10)]
        [SerializeField] private List<PreyActionListener> _listeners;
        [SerializeField] private List<PreySurprisedListener> _surprisedListeners;
        private IPreyHealth _health;
        
        public PreySettings PreySettings
        {
            get => _settings;
            set => _settings = value;
        }
        
        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.AddListener(this);
            foreach (var listener in _listeners)
                listener.OnInit();
        }

        public float GetReward() => _settings.Reward;
        
        public void SurpriseToAttack()
        {
            foreach (var surprisedListener in _surprisedListeners)
                surprisedListener.OnSurprised();
        }
        
        public void IdleState()
        {
            // _preyAnimator.RandomIdle();
        }
        
        public void RunState()
        {
            foreach (var listener in _listeners)
                listener.OnBeganRun();
        }
        

        public void OnHealthChange(float health, float maxHealth)
        {
            if(health <= 0)
                OnDead();
        }
        
        private void OnDead()
        {
            transform.SetParent(null);
            _health.Hide();
            foreach (var listener in _listeners)
                listener.OnDead();
            OnKilled?.Invoke(this);
        }
    }
}