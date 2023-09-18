using System;
using Common.Ragdoll;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class Prey : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private ParticleSystem _preyParticles;
        [SerializeField] private PreySettings _settings;
        private IPreyHealth _health;
        
        
        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.OnDead += OnDead;
        }

        public Vector3 GetPosition() => transform.position;
        public Quaternion GetRotation() => transform.rotation;
        
        public float GetReward() => _settings.Reward;

        public void Activate()
        {
            _preyAnimator.Run();
            _health.Show();
        }

        private void OnDead()
        {
            _collidersSwitch.Off();
            _preyAnimator.Disable();
            _ragdoll.Activate();
            _health.Hide();
            StopParticles();
            OnKilled?.Invoke(this);
        }
        
        private void StopParticles()
        {
            _preyParticles.Stop();
            _preyParticles.Clear();
        }
    }
}