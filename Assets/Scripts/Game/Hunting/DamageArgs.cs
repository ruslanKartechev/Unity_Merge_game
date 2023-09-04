using UnityEngine;

namespace Game.Hunting
{
    public struct DamageArgs
    {
        public DamageArgs(float damage, Vector3 position)
        {
            Damage = damage;
            Position = position;
        }

        public float Damage;
        public Vector3 Position;
    }
}