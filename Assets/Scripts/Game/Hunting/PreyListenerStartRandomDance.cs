using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerStartRandomDance : PreyActionListener
    {
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private List<string> _animations;

        public override void OnStarted()
        {
            var randomName = _animations.Random();
            _animator.PlayByName(randomName);
        }

        public override void OnDead()
        { }

        public override void OnBeganRun()
        { }
    }
}