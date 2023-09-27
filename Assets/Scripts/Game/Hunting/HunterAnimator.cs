﻿using UnityEngine;

namespace Game.Hunting
{
    public class HunterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        [SerializeField] private string _jumpKey;
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }
#endif

        public void Idle()
        {
            _animator.Play(_idleKey);
        }

        public void Run()
        {
            _animator.Play(_runKey);
        }

        public void Jump()
        {
            _animator.Play(_jumpKey);
        }

        public void Disable() => _animator.enabled = false;
    }
}