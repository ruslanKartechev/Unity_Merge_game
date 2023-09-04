using UnityEngine;

namespace Game.Hunting
{
    public interface IDamageable
    {
        void Damage(DamageArgs damageArgs);
    }

    public interface IBiteTarget : IDamageable
    {
        Transform GetBiteBone();
    }
}