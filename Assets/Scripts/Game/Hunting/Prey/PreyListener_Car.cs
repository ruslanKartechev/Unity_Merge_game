using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListener_Car : PreySurprisedListener, IHealthListener
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private CarParticles _carParticles;
        [SerializeField] private SidewaysDir _rotDir;
        [Space(10)]
        [SerializeField] private float _rotTime = 1f;
        
        
        public override void OnInit()
        {
            // _health.AddListener(this);
        }

        public override void OnDead()
        {
            _preyAnimator.Disable();
            _collidersSwitch.Off();
            _carWheelsController.StopAll();
            _partsDestroyer.DestroyAllParts();
            _carParticles.Hide();
        }

        public override void OnBeganRun()
        {
            _carWheelsController.StopAll();
            _carWheelsController.StartMoving();
            // _preyAnimator.Moving();
            _health.Show();
            _localMover.MoveToLocalPoint();
            _carParticles.Spawn();
            _carParticles.Play();
        }

        public override void OnSurprised()
        {
            _localMover.RotateToPoint();
            if(_rotDir == SidewaysDir.Right)
                _carWheelsController.RotateToRightAndBack(_rotTime);
            else 
                _carWheelsController.RotateToLeftAndBack(_rotTime);
        }

        public void OnHealthChange(float health, float maxHealth)
        {
            if (health >= maxHealth)
                return;
            _partsDestroyer.DestroyWindow();
        }
    }

}