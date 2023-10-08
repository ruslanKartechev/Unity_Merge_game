using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/Shop/" + nameof(ShopItems), fileName = nameof(ShopItems), order = 5)]
    public class ShopItems : ScriptableObject, IShopItems, IShopSettingsRepository
    {
        [SerializeField] private List<ShopItem> _shopItems;
        [SerializeField] private List<ShopSettings> _shopSettings;

        public int Count => _shopItems.Count;
        
        public IShopItem GetItem(int index)
        {
            return _shopItems[index];
        }

        public IShopSettings GetSettings(int level)
        {
            if (level >= _shopItems.Count)
                return null;
            return _shopSettings[level];
        }
    }
}