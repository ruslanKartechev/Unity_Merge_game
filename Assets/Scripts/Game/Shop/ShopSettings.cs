using Game.Merging;
using UnityEngine;

namespace Game.Shop
{
    // [CreateAssetMenu(menuName = "SO/Shop/" + nameof(ShopSettings), fileName = nameof(ShopSettings), order = 0)]
    [System.Serializable]
    public class ShopSettings : IShopSettings
    {
        [SerializeField] private int _maxLevel = -1;
        [SerializeField] private MergeItemSO _item;

        public int MaxLevel => _maxLevel;

        public MergeItem OutputItem
        {
            get => _item == null ? null : _item.Item;
        }
    }
}