using System;
using System.Collections;
using Common.Ragdoll;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class Prey : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private float _rotTime = 1f;
        [SerializeField] private float _runAnimationSpeed = 1f;
        [Space(10)]
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private ParticleSystem _preyParticles;
        [SerializeField] private PreySettings _settings;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        private IPreyHealth _health;
        
        
        public void Init()
        {
            _health = gameObject.GetComponent<IPreyHealth>();
            _health.Init(_settings.Health);
            _health.OnDead += OnDead;
            _preyAnimator.SetRunAnimationSpeed(_runAnimationSpeed);
        }

        public float GetReward() => _settings.Reward;

        public ICamFollowTarget CamTarget => _camFollowTarget;

        public void IdleState()
        {
            _preyAnimator.Idle();
        }
        
        public void RunState()
        {
            _preyAnimator.SetRunAnimationSpeed(_runAnimationSpeed);
            _preyAnimator.Run();
            _health.Show();
            StartCoroutine(RotatingToLocal());
        }

        private void OnDead()
        {
            transform.SetParent(null);
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
        
        private IEnumerator RotatingToLocal()
        {
            var elapsed = 0f;
            var rot1 = transform.localRotation;
            var rot2 = Quaternion.identity;
            var time = _rotTime;
            while (elapsed <= time)
            {
                transform.localRotation = Quaternion.Lerp(rot1, rot2, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
        }
    }
}