using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerBarrel: PreySurprisedListener
    {
        [SerializeField] private string _idleName;
        [SerializeField] private string _throwName;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private PreyListener_Barbarian _surpriseToAttack;
        [SerializeField] private BarrelThrowable _barrel;
        [SerializeField] private PackUnitLocalMover _localMover;

        
        public override void OnInit()
        {
            _animator.PlayByName(_idleName);
        }
        
        public override void OnDead()
        { }
        
        public override void OnBeganRun()
        { }

        public override void OnSurprised()
        {
            _animator.TriggerByName(_throwName);
        }

        // Animation Event
        public void OnBarrelThrown()
        {
            ThrowBarrel();
            _localMover.RotateToPoint();
            _surpriseToAttack.OnBeganRun();
        }

        private void ThrowBarrel()
        {
            _barrel.Push();
        }
    }
}