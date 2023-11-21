using UnityEngine;

namespace Game.Hunting.Prey.Interfaces
{
    public interface IFishTarget : IDamageable
    {
        Vector3 GetShootAtPosition();
    }
}