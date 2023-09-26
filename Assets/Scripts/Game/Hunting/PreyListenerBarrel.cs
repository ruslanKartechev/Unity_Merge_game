using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerBarrel: PreySurprisedListener
    {
        [SerializeField] private string _idleName;
        [SerializeField] private string _throwName;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private PreyListener_SurpriseToAttack _surpriseToAttack;
        [SerializeField] private BarrelThrowable _barrel;
        
        public override void OnStarted()
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
            _surpriseToAttack.RotateToLocal();
            _surpriseToAttack.OnBeganRun();
        }

        private void ThrowBarrel()
        {
            _barrel.Push();
        }
    }
}