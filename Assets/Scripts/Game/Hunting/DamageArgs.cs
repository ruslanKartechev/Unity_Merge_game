using UnityEngine;

namespace Game.Hunting
{
    public struct DamageArgs
    {
        public DamageArgs(float damage, Vector3 position)
        {
            Damage = damage;
            Position = position;
            Direction = Vector3.up;
        }
        
        public DamageArgs(float damage, Vector3 position, Vector3 direction)
        {
            Damage = damage;
            Position = position;
            Direction = direction;
        }


        public float Damage;
        public Vector3 Position;
        public Vector3 Direction;
    }
}