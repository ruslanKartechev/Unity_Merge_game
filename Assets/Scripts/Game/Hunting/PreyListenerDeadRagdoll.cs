using Common.Ragdoll;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerDeadRagdoll : PreyActionListener
    {
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private PreyAnimator _preyAnimator;

        public override void OnStarted()
        { }

        public override void OnDead()
        {
            _collidersSwitch.Off();
            _preyAnimator.Disable();
            _ragdoll.Activate();
        }

        public override void OnBeganRun()
        { }
    }
}