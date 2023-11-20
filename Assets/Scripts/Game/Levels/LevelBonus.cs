using Game.Merging;
using Game.Shop;
using UnityEngine;

namespace Game.Levels
{
    [CreateAssetMenu(menuName = "SO/Level/" + nameof(LevelBonus), fileName = nameof(LevelBonus), order = 0)]
    public class LevelBonus : ScriptableObject
    {
        public enum BonusType
        {
            None,
            Egg,
            Boss,
            Money
        }

        [SerializeField] private BonusType _bonusType;
        [SerializeField] private float _rewardAmount;
        [SerializeField] private Sprite _uiSprite;
        [SerializeField] private MergeItemSO _bonusItem;
        
        public BonusType Type => _bonusType;
        public Sprite Sprite => _uiSprite;
        public float RewardAmount => _rewardAmount;
        public MergeItemSO Item => _bonusItem;

    }
}