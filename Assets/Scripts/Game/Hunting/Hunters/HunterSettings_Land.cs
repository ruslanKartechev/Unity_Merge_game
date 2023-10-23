using Game.Hunting.Hunters.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    [System.Serializable]
    public class HunterSettings_Land : IHunterSettings
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _jumpSpeed = 1;
        [SerializeField] private float _biteCastRadius = 0.33f;

        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public float Radius => _biteCastRadius;
    }
    
    
    [System.Serializable]
    public class HunterSettings_Kong : IHunterSettings_Kong
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _jumpSpeed = 1;
        [SerializeField] private float _biteCastRadius = 0.33f;

        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public float Radius => _biteCastRadius;
    }
}