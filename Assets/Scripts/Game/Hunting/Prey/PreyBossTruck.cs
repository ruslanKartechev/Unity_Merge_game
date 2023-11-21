using System;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBossTruck : MonoBehaviour, IPrey, IPredatorTarget, IFishTarget, IAirTarget
    {
        public event Action<IPrey> OnKilled;

        [SerializeField] private PreySettings _settings;
        [SerializeField] private CarWheelsController _wheelsController;
        [SerializeField] private KongAnimator _kongAnimator;
        [SerializeField] private BindingRopes _bindingRopes;
        [Space(10)]
        [SerializeField] private Transform _airTarget;
        [SerializeField] private Transform _fishShootTarget;
        [Space(10)]        
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        private PreyHealth _health;

        public PreySettings PreySettings
        {
            get => _settings;
            set => _settings = value;
        }

        public ICamFollowTarget CamTarget => _camFollowTarget;
        public bool IsAvailableTarget => true;

        public void Init()
        {
            _health = gameObject.GetComponent<PreyHealth>();
            _health.Init(_settings.Health);
            _bindingRopes.InitHealthPoints();
        }

        public float GetReward() => _settings.Reward;
        public bool IsAlive() => _health.IsAlive();
        public Vector3 GetShootAtPosition() => _fishShootTarget.position;
        public bool CanBindTo() => false;
        public bool CanGrabToAir() => false;
        public void OnPackAttacked()
        { }

        public void OnPackRun()
        {
            _wheelsController.StartMoving();
        }
        
        public void Damage(DamageArgs damageArgs)
        {
            _health.Damage(damageArgs);
            _bindingRopes.DropToHealth(_health.Percent);
            if (_health.IsAlive() == false)
                Die();
        }


        private void Die()
        {
            _wheelsController.StopAll();
            transform.SetParent(null);
            _kongAnimator.transform.SetParent(null);
            _kongAnimator.KongFree();
            _health.Hide();
            _partsDestroyer.DestroyAllParts();
            OnKilled?.Invoke(this);
        }

        public Transform GetFlyToTransform() => _airTarget;

        public Transform MoverParent() => transform.parent;

        public void GrabTo(Transform transform, DamageArgs damage)
        {}

        public void DropAlive()
        {}

        public void DropDead()
        {}
    }
}