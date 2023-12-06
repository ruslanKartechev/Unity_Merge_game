using System;
using System.Collections.Generic;
using Common.Ragdoll;
using Game.Hunting;
using UnityEngine;

namespace Creatives.Firemen
{
    public interface IPushBackTarget
    {
        void PushBack(Vector3 fromPoint);
    }
    public class FiremanTarget : MonoBehaviour, IPushBackTarget
    {
        public Mode mode;
        public bool explode;
        public bool dropWeapon;
        public float force_min;
        public float force_max;
        public float otherForce;
        public IRagdoll ragdoll;
        public Animator animator;
        public List<ParticleSystem> explosions;
        public List<GameObject> weapons;
        [Header("Idle")] 
        public string idleAnim;
        public bool disableIdleParticles;
        public List<ParticleSystem> idleParticles;

        public enum Mode
        {
            None,
            Random,
            DiePushOut,
            DiePushUp,
            DieInPlace
        }

        private void Awake()
        {
            animator.SetFloat("AnimationOffset", UnityEngine.Random.Range(0f, 1f));
            animator.Play(idleAnim);
        }

        public void PushBack(Vector3 fromPoint)
        {
            switch (mode)
            {
                case Mode.None:
                    Debug.Log("none");
                    return;
                    break;
                case Mode.Random:
                    Debug.Log($"{gameObject.name} Random");
                    break;
                case Mode.DiePushOut:
                    Debug.Log($"{gameObject.name} PushOut");
                    Out(fromPoint);
                    break;
                case Mode.DiePushUp:
                    Debug.Log($"{gameObject.name} PushUp");
                    Up(fromPoint);
                    break;
                case Mode.DieInPlace:
                    Debug.Log($"{gameObject.name} InPlace");
                    InPlace();
                    break;
            }    
        }

        private void InPlace()
        {
            animator.enabled = false;
            ragdoll.Activate();
            Particles();
            Weapon();
        }
        
        private void Out(Vector3 from)
        {
            animator.enabled = false;
            from = transform.position - from;
            from.Normalize();
            var force = from * Force() + Vector3.up * otherForce;
            ragdoll.ActivateAndPush(force);
            Particles();
            Weapon();   
        }

        private void Up(Vector3 from)
        {
            animator.enabled = false;
            from = transform.position - from;
            from.Normalize();
            var force = from * otherForce + Vector3.up * Force();
            ragdoll.ActivateAndPush(force);
            Particles();
            Weapon();   
        }

        private float Force()
        {
            return UnityEngine.Random.Range(force_min, force_max);
        }

        private void Particles()
        {
            if (disableIdleParticles)
            {
                foreach (var ps in idleParticles)
                {
                    ps.gameObject.SetActive(false);
                }   
            }
            if (!explode)
                return;
            foreach (var ps in explosions)
            {
                ps.transform.parent = null;
                ps.gameObject.SetActive(true);
                ps.Play();
            }
        }

        private void Weapon()
        {
            if (!dropWeapon)
                return;
            foreach (var weapon in weapons)
            {
                if(weapon == null)
                    continue;
                if (weapon.TryGetComponent<ColdWeapon>(out var w))
                {
                    w.Drop();
                }
            }
        }
        
    }
}