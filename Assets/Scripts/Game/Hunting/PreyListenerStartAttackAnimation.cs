﻿using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerStartAttackAnimation : PreySurprisedListener
    {
        [SerializeField] private string _animationName;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private TreeAnimator _treeAnimator;
        
        public override void OnInit()
        {
            _animator.PlayByName(_animationName);   
        }

        public override void OnDead()
        {
            _treeAnimator.StopAnimator();
            _treeAnimator.transform.parent = null;   
        }

        public override void OnBeganRun()
        { }

        public override void OnSurprised()
        {
            _treeAnimator.StopAnimator();
        }
    }
}