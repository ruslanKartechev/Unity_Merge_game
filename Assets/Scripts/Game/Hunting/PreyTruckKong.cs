using System;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class KongAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _freeKey;

        public void Idle()
        {
        }

        public void Free()
        {
            _animator.SetTrigger(_freeKey);
        }
    }
    public class PreyTruckKong : MonoBehaviour, IPrey, IHealthListener
    {
        public event Action<IPrey> OnKilled;

        [SerializeField] private ParticleSystem _preyParticles;
        [SerializeField] private PreySettings _settings;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private CarWheelsController _wheelsController;
        [SerializeField] private KongAnimator _kongAnimator;
        [SerializeField] private BindingRopes _bindingRopes;
        private IPreyHealth _health;
        
        
        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.AddListener(this);
        }

        public float GetReward() => _settings.Reward;
        
        public void SurpriseToAttack()
        {
            
        }

        public ICamFollowTarget CamTarget => _camFollowTarget;

        public void IdleState()
        {
            _kongAnimator.Idle();
        }
        
        public void RunState()
        {
            _wheelsController.StartMoving();
        }

        private void StopParticles()
        {
            if(_preyParticles != null)
                _preyParticles.Stop();
        }

        public void OnHealthChange(float health, float maxHealth)
        {
            if(health <= 0)
                OnDead();
            else
            {
                var fraction = health / maxHealth;
                   
            }
        }
        
        private void OnDead()
        {
            transform.SetParent(null);
            _health.Hide();
            StopParticles();
            OnKilled?.Invoke(this);
        }
    }
}