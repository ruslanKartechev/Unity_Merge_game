﻿using System;
using System.Collections;
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
        [Space(10)] 
        [SerializeField] private float _moneyFlyDelay;   
        [SerializeField] private Transform _moneyPoint;
        [SerializeField] private bool _useFlyMoney = true;
        [SerializeField] private float _moneyReward = 100f;

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
            DropWeapon();
            FlyMoney();
        }
        
        private void Out(Vector3 from)
        {
            animator.enabled = false;
            from = transform.position - from;
            from.Normalize();
            var force = from * Force() + Vector3.up * otherForce;
            ragdoll.ActivateAndPush(force);
            Particles();
            DropWeapon();
            FlyMoney();
        }

        private void Up(Vector3 from)
        {
            animator.enabled = false;
            from = transform.position - from;
            from.Normalize();
            var force = from * otherForce + Vector3.up * Force();
            ragdoll.ActivateAndPush(force);
            Particles();
            DropWeapon();
            FlyMoney();
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

        private void DropWeapon()
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
        
        private void FlyMoney()
        {
            if (_moneyFlyDelay <= 0)
            {
                CallMoney();
                return;
            }

            StartCoroutine(DelayedMoney(_moneyFlyDelay));

        }

        private void CallMoney()
        {
            if (_useFlyMoney && _moneyPoint != null)
            {
                var creos = CreosUI.Instance;
                if (creos == null)
                    return;
                creos.FlyingMoney.FlySingle(_moneyPoint.position, _moneyReward);
            }    
        }
        
        private IEnumerator DelayedMoney(float time)
        {
            yield return new WaitForSeconds(time);
            CallMoney();
        }
        
    }
}