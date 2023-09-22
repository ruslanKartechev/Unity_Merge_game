using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerBarrel: PreySurprisedListener
    {
        [SerializeField] private string _idleName;
        [SerializeField] private string _throwName;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private PreyListenerSurpriseToAttack _surpriseToAttack;
        [SerializeField] private BarrelThrowable _barrel;
        
        public override void OnStarted()
        {
            Debug.Log($"Playing animation: {_idleName}");
            _animator.PlayByName(_idleName);
        }
        
        public override void OnDead()
        { }
        
        public override void OnBeganRun()
        { }

        public override void OnSurprised()
        {
            Debug.Log($"Playing animation: {_throwName}");
            _animator.TriggerByName(_throwName);
        }

        // Animation Event
        public void OnBarrelThrown()
        {
            Debug.Log("BARREL THROWN");
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