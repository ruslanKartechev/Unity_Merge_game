using UnityEngine;

namespace Game.Hunting
{
    public interface IPredatorTarget : IDamageable
    {
        bool CanBindTo();
    }
    
    public interface IFishTarget : IDamageable
    {
        Vector3 GetShootAtPosition();
    }
}