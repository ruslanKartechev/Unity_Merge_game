using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(DefaultMergeItemStats), fileName = nameof(DefaultMergeItemStats), order = 10)]
    public abstract class DefaultMergeItemStats : MergeItemStats, IMergeItemStats
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _attackSpeed;

        public float Damage => _damage;
        public float AttackRange => _attackRange;
        public float AttackRadius => _attackRadius;
        public float AttackSpeed => _attackSpeed;
    }
}