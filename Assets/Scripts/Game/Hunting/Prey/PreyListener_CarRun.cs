using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListener_CarRun : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private CarParticles _carParticles;
        public event Action OnEnded;
        
        public void Begin()
        {
            _carWheelsController.StopAll();
            _carWheelsController.StartMoving();
            // _preyAnimator.Moving();
            _health.Show();
            _localMover.MoveToLocalPoint();
            _carParticles.Spawn();
            _carParticles.Play();
        }

        public void Stop()
        { }
    }
}