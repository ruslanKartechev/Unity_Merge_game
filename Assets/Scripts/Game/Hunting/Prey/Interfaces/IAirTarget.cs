using UnityEngine;

namespace Game.Hunting.Prey.Interfaces
{
    public interface IAirTarget : IDamageable
    {
        bool CanGrabToAir();
        Transform GetFlyToTransform();
        Transform MoverParent();
        void GrabTo(Transform transform);
        void DropAlive();
        void DropDead();
        
    }
}