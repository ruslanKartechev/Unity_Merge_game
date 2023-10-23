using UnityEngine;

namespace Game.Hunting
{
    public interface IAirTarget : IDamageable
    {
        bool CanBindTo();
        Transform GetFlyToTransform();
        Transform MoverParent();
        void GrabTo(Transform transform);
        void DropAlive();
        void DropDead();
        
    }
}