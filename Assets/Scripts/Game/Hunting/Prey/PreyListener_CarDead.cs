using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListener_CarDead : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private CarParticles _carParticles;
        public event Action OnEnded;
        
        public void Begin()
        {
            transform.parent.parent.SetParent(null);
            _preyAnimator.Disable();
            _collidersSwitch.Off();
            _carWheelsController.StopAll();
            _partsDestroyer.DestroyAllParts();
            _carParticles.Hide();
            _localMover.StopMoving();
        }

        public void Stop()
        { }
    }
}