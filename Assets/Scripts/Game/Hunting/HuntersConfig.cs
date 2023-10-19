using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HuntersConfig), fileName = nameof(HuntersConfig), order = 0)]
    public class HuntersConfig : ScriptableObject
    {
        [SerializeField] private float _afterAttackDelay = .666f;
        [SerializeField] private float _jumpTMax = .95f;
        [SerializeField] private float _maxSlowMoTime = .4f;
        [SerializeField] private LayerMask _biteMask;
        [SerializeField] private LayerMask _fishDamageMask;
        [SerializeField] private LayerMask _airDamageMask;

        public float JumpTMax => _jumpTMax;
        public float AfterAttackDelay => _afterAttackDelay;
        public float MaxSlowMoTime => _maxSlowMoTime;
        public LayerMask BiteMask => _biteMask;
        public LayerMask FishDamageMask => _fishDamageMask;
        public LayerMask AirDamageMask => _airDamageMask;
        
        
    }
}