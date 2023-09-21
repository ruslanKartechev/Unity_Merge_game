using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerStartAttackAnimation : PreyActionListener
    {
        [SerializeField] private string _animationName;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private TreeAnimator _treeAnimator;
        
        public override void OnStarted()
        {
            _animator.Attack(_animationName);   
        }

        public override void OnDead()
        { }

        public override void OnBeganRun()
        {
            _treeAnimator.Stop();
            _treeAnimator.transform.parent = null;
        }
    }
}