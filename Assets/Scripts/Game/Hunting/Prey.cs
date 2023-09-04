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
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private Animator _animator;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private ParticleSystem _preyParticles;
        private IPreyMover _mover;
        private IPreySettings _settings;
        private IPreyHealth _health;
        
        
        public void Init(IPreySettings settings, SplineComputer path)
        {
            _settings = settings;
            _mover = GetComponent<IPreyMover>();
            _mover.Init(_settings, path);
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.OnDead += OnDead;
        }

        public Vector3 GetPosition() => transform.position;
        public Quaternion GetRotation() => transform.rotation;
        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        
        public float GetReward() => _settings.Reward;

        public void Activate()
        {
            _mover.BeginMoving();
            _health.Show();
        }

        private void OnDead()
        {
            _collidersSwitch.Off();
            _mover.StopMoving();
            _animator.enabled = false;
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