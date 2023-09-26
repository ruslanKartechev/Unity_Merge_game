using UnityEngine;

namespace Game.Hunting
{
    public class PreyListener_Bike : PreySurprisedListener
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private SidewaysDir _rotDir;
        [Space(10)]
        [SerializeField] private float _rotTime = 1f;
        [Space(10)] 
        [SerializeField] private CarParticles _carParticles;
        [SerializeField] private PreyListenerDeadRagdoll _ragdoll;        
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        
        
        public override void OnInit()
        { 
            _carParticles.Spawn();
        }

        public override void OnDead()
        {
            _ragdoll.OnDead();   
            _partsDestroyer.DestroyAllParts();
        }

        public override void OnBeganRun()
        {
            _carWheelsController.StopAll();
            _carWheelsController.StartMoving();
            _preyAnimator.Moving();
            _health.Show();
            _carParticles.Play();
            _localMover.MoveToLocalPoint();
        }

        public override void OnSurprised()
        {
            _localMover.RotateToPoint();
            if(_rotDir == SidewaysDir.Right)
                _carWheelsController.RotateToRightAndBack(_rotTime);
            else 
                _carWheelsController.RotateToLeftAndBack(_rotTime);
        }
  
    }
}