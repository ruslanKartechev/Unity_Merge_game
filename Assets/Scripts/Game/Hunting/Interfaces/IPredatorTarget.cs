using UnityEngine;

namespace Game.Hunting
{
    public interface IPredatorTarget : IDamageable
    {
        bool CanBite();
        Transform GetBiteParent();
    }
}