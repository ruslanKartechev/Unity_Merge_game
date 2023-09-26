using Common.Ragdoll;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerDeadRagdoll : PreyActionListener
    {
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private bool _pushRagdoll;
        [SerializeField] private RagdollBodyPusher _pusher;

        
        public override void OnInit()
        { }

        public override void OnDead()
        {
            _collidersSwitch.Off();
            _preyAnimator.Disable();
            _ragdoll.Activate();
            if(_pushRagdoll)
                _pusher.Push(transform.forward);
        }

        public override void OnBeganRun()
        { }
    }
}