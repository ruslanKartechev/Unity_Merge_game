using System;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListener_Car : PreySurprisedListener
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private LocalRotator _localRotator;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private SidewaysDir _rotDir;
        [Space(10)]
        [SerializeField] private float _rotTime = 1f;
        
        
        public override void OnInit()
        {
        }

        public override void OnDead()
        {
        }

        public override void OnBeganRun()
        {
            _carWheelsController.StopAll();
            _carWheelsController.StartMoving();
            _preyAnimator.Moving();
            _health.Show();
        }

        public override void OnSurprised()
        {
            _localRotator.RotateTo(Quaternion.identity, _rotTime);
            if(_rotDir == SidewaysDir.Right)
                _carWheelsController.RotateToRightAndBack(_rotTime);
            else 
                _carWheelsController.RotateToLeftAndBack(_rotTime);
        }
  
    }
}