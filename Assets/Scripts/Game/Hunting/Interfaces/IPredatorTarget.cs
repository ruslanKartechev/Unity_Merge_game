using UnityEngine;

namespace Game.Hunting
{
    public interface IPredatorTarget : IDamageable
    {
        bool IsBiteable();
    }
    
    public interface IFishTarget : IDamageable
    {
        Vector3 GetShootAtPosition();
    }

    public interface IAirTarget : IDamageable
    {
        
    }


}