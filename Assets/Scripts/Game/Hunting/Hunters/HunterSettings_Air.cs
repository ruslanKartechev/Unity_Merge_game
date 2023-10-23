using Game.Hunting.Hunters.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    [System.Serializable]
    public class HunterSettingsAir : IHunterSettings_Air
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _jumpSpeed = 1;
        [SerializeField] private float _radius = 0.33f;
        [Space(10)] 
        [SerializeField] private float _minDistance;
        [SerializeField] private float _toBiteFlyTime;
        
        
        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public float Radius => _radius;
        public float MinDistance() => _minDistance;
        public float ToBitePosFlyTime() => _toBiteFlyTime;
    }
}