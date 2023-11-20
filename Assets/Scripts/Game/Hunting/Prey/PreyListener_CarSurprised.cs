using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListener_CarSurprised : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private float _rotTime = 1f;
        [SerializeField] private SidewaysDir _rotDir;

        public event Action OnEnded;
        
        public void Begin()
        {
            _localMover.RotateToPoint();
            if(_rotDir == SidewaysDir.Right)
                _carWheelsController.RotateToRightAndBack(_rotTime);
            else 
                _carWheelsController.RotateToLeftAndBack(_rotTime);
        }

        public void Stop()
        { }
    }
}