using System;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives
{
    public class CreativeAnimal : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private PreySettings _settings;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _startKey = "Run";
        [SerializeField] private string _offsetKey = "Offset";
        
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
        public ICamFollowTarget CamTarget => _camFollowTarget;
        public bool IsAvailableTarget { get; private set; }
    }
}