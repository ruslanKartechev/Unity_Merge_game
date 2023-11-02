using System;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_BikeIdle : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private CarParticles _carParticles;
        public event Action OnEnded;

        public void Begin()
        {
            _preyAnimator.PlayByName("Idle_1");
            _carParticles.Spawn();
        }

        public void Stop()
        { }
    }
}