using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ShopItems), fileName = nameof(ShopItems), order = 5)]
    public class ShopItems : ScriptableObject, IShopItems
    {
        [SerializeField] private List<ShopItem> _shopItems;
        
        
        public int Count => _shopItems.Count;
        
        public IShopItem GetItem(int index)
        {
            return _shopItems[index];
        }
    }
}