using System;
using Common.Ragdoll;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives
{
    public class CreativeAnimal : MonoBehaviour, IPrey, IPredatorTarget
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private PreySettings _settings;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _startKey = "Run";
        [SerializeField] private string _offsetKey = "Offset";
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private ParticleSystem _blood;
        
        public void Init()
        {
            IsAvailableTarget = true;
            PreySettings = _settings;
        }

        public void OnPackRun()
        {
            _animator.SetFloat(_offsetKey, UnityEngine.Random.Range(0f, 1f));
            _animator.Play(_startKey);
        }

        public void OnPackAttacked()
        {
        }

        public float GetReward()
        {
            return 100;
        }

        public PreySettings PreySettings { get; set; }
        [SerializeField]  private bool _isAvailable = true;
        private bool _isAlive = true;
        public ICamFollowTarget CamTarget => _camFollowTarget;
        public bool IsAvailableTarget
        {
            get => _isAvailable;
            private set
            {
                _isAvailable = value;
            }
        }


        public void Damage(DamageArgs damageArgs)
        {
            if (_blood != null)
            {
                _blood.gameObject.SetActive(true);
                _blood.Play();
            }
            Die();
        }

        public bool IsAlive()
        {
            return _isAlive;
        }

        public bool CanBindTo()
        {
            return true;
        }

        public void Die()
        {
            transform.SetParent(null);
            _animator.enabled = false;
            _ragdoll.Activate();       
        }
    }
}