using System;
using Common.Ragdoll;
using Game.Hunting;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives.Office
{
    public class OfficeHuman : MonoBehaviour, IPredatorTarget
    {
        public event Action OnDead;
        public float pushForce;
        public float pushForceUp;
        public float health;
        public bool IsKillable;
        public bool CanBite = true;
        public Animator animator;
        public IRagdoll ragdoll;
        private bool _isAlive = true;
        private DamageArgs _damageArgs;
        
        public void Damage(DamageArgs damageArgs)
        {
            if (!IsKillable)
                return;
            _damageArgs = damageArgs;
            health -= damageArgs.Damage;
            if (health <= 0)
            {
                Die();   
            }
        }

        public bool IsAlive()
        {
            return _isAlive;
        }

        public bool CanBindTo()
        {
            return CanBite;
        }

        public void DollDie()
        {
            _isAlive = false;
            IsKillable = false;
            animator.enabled = false;
            var dir = _damageArgs.Direction;
            dir.y = 0f;
            var force = dir.normalized * pushForce + Vector3.up * pushForceUp;
            ragdoll.ActivateAndPush(force);
        }

        private void Die()
        {
            OnDead?.Invoke();   
        }
    }
}