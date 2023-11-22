using System.Collections.Generic;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/Shop/" + nameof(ShopItem), fileName = nameof(ShopItem), order = 9)]
    public class ShopItem : ScriptableObject, IShopItem
    {
        [SerializeField] private string _itemId;
        [SerializeField] private CostByLevelPicker _costPicker;
        [SerializeField] private int _itemLevel;

        [Space(10)] 
        [SerializeField] private List<ShopItemOutput> _shopItemOutputs;

        public string ItemId => _itemId;
        public float Cost => _costPicker.GetCost(GC.PlayerData.LevelTotal + 1);
        public int ItemLevel => _itemLevel;

        public IList<ShopItemOutput> Outputs => _shopItemOutputs;


    }
}