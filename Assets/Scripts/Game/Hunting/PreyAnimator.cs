﻿using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        [SerializeField] private string _injuredKey;
        [SerializeField] private string _damageKey;
        [Space(10)] 
        [SerializeField] private Vector2 _runOffsetLimits;
        [Space(10)]
        [SerializeField] private List<AnimatorOverrideController> _controllerOverrides;
        [SerializeField] private float _healthPercentToInjure = .5f;
        private bool _isInjuredAnim;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }
#endif

        public void Disable()
        {
            _animator.enabled = false;
        }

        public void SetRunAnimationSpeed(float speed)
        {
            _animator.SetFloat("RunSpeed", speed);
        }
        
        public void RandomIdle()
        {
            RandomizeController();
            _animator.SetFloat("AnimationOffset", _runOffsetLimits.Random());
            _animator.Play(_idleKey);
        }

        public void PlayByName(string animName)
        {
            _animator.Play(animName);
        }
        
        public void TriggerByName(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        
        public void Moving()
        {
            RandomizeController();
            _animator.SetFloat("AnimationOffset", _runOffsetLimits.Random());
            _animator.SetTrigger(_runKey);
        }

        public void Surprise()
        {
            _animator.SetTrigger("Surprised");
        }
        
        public void Injured(float health)
        {
            if (_isInjuredAnim)
                return;
            if (health <= _healthPercentToInjure)
            {
                _isInjuredAnim = true;
                _animator.SetTrigger(_injuredKey);
            }
        }

        public void Damage()
        {
            _animator.SetTrigger(_damageKey);
        }
        
        private void RandomizeController()
        {
            if (_controllerOverrides.Count == 0)
                return;
            _animator.runtimeAnimatorController = _controllerOverrides.Random();
        }

    }
}