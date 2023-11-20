using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_BikeDead : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private CarWheelsController _carWheelsController;
        [SerializeField] private CollidersSwitch _collidersSwitch;
        [SerializeField] private CarParticles _carParticles;
        [SerializeField] private PreyListenerDeadRagdoll _ragdoll;        
        [SerializeField] private CarPartsDestroyer _partsDestroyer;
        public event Action OnEnded;

        public void Begin()
        {
            transform.parent.parent.SetParent(null);
            _collidersSwitch.Off();
            _carWheelsController.StopAll();
            _ragdoll.OnDead();   
            _partsDestroyer.DestroyAllParts();
            _carParticles.Hide();
        }

        public void Stop()
        { }
    }
}