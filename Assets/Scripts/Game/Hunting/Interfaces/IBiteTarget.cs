using UnityEngine;

namespace Game.Hunting
{
    public interface IBiteTarget : IDamageable
    {
        Transform GetBiteParent();
        Transform GetClosestBitePosition(Vector3 point);
    }
}