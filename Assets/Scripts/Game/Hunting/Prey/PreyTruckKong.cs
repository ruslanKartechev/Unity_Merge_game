using System;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyTruckKong : MonoBehaviour, IPrey, IHealthListener
    {
        public event Action<IPrey> OnKilled;

        [SerializeField] private PreySettings _settings;
        [SerializeField] private CarWheelsController _wheelsController;
        [SerializeField] private KongAnimator _kongAnimator;
        [SerializeField] private BindingRopes _bindingRopes;
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        private IPreyHealth _health;

        public PreySettings PreySettings
        {
            get => _settings;
            set => _settings = value;
        }

        public ICamFollowTarget CamTarget => _camFollowTarget;

        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            // _health.Init(_settings.Health);
            // _health.AddListener(this);
            _bindingRopes.InitHealthPoints();
        }
        

        public float GetReward() => _settings.Reward;
        
        public void OnPackAttacked()
        { }

        public void IdleState()
        {}
        
        public void OnPackRun()
        {
            _wheelsController.StartMoving();
            // _health.Show();
        }

        private void StopParticles()
        { }

        public void OnHealthChange(float health, float maxHealth)
        {
            var percent = health / maxHealth;
            _bindingRopes.DropToHealth(percent);
            if(health <= 0)
                OnDead();
        }
        
        private void OnDead()
        {
            _wheelsController.StopAll();
            transform.SetParent(null);
            _kongAnimator.transform.SetParent(null);
            _kongAnimator.KongFree();
            // _health.Hide();
            _partsDestroyer.DestroyAllParts();
            StopParticles();
            OnKilled?.Invoke(this);
        }
    }
}