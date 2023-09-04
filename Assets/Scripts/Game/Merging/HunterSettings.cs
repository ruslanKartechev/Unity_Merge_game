using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class HunterSettings : IHunterSettings
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _jumpSpeed = 1;
        [SerializeField] private LayerMask _preyMask;
        [SerializeField] private float _biteDistance = 0.25f;
        [SerializeField] private float _biteOffset = 0.1f;

        public float Damage => _damage;
        public float JumpSpeed => _jumpSpeed;
        public LayerMask BiteMask => _preyMask;
        public float BiteDistance => _biteDistance;
        public float BiteOffset => _biteOffset;
    }
}