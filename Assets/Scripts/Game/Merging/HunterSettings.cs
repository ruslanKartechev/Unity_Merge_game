using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class HunterSettings : IHunterSettings
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _jumpSpeed = 1;
        [SerializeField] private float _biteCastRadius = 0.33f;

        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public float BiteCastRadius => _biteCastRadius;
    }
}