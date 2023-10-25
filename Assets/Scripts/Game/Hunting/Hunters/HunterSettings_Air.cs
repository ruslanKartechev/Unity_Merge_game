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
        [SerializeField] private Vector3 _flyAwayOffset;
        [SerializeField] private float _flyAwayDuration;
        [SerializeField] private float _liftUpDuration;
        [SerializeField] private float _dropAliveDelay;
        [SerializeField] private float _liftUpHeight;
        
        
        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public float Radius => _radius;

        public Vector3 FlyAwayOffset => _flyAwayOffset;
        public float FlyAwayDuration => _flyAwayDuration;
        public float LiftUpDuration => _liftUpDuration;
        public float LiftUpHeight => _liftUpHeight;
        public float DropAliveDelay => _dropAliveDelay;
    }
}