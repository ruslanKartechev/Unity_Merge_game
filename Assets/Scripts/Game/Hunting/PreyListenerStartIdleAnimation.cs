﻿using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerStartIdleAnimation : PreyActionListener
    {
        [SerializeField] private PreyAnimator _animator;
        
        
        public override void OnStarted()
        {
            _animator.RandomIdle();
        }

        public override void OnDead()
        { }

        public override void OnBeganRun()
        { }
    }
}