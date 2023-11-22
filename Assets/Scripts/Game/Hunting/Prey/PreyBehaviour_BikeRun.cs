using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_BikeRun : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private CarParticles _carParticles;
        public event Action OnEnded;

        public void Begin()
        {
            _carWheelsController.StopAll();
            _carWheelsController.StartMoving();
            _preyAnimator.PlayByName("Idle");
            _health.Show();
            _carParticles.Play();
            _localMover.MoveToLocalPoint();
        }

        public void Stop()
        { }
    }
}