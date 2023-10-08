using System;
using System.Collections.Generic;
using Game.Merging;
using UnityEngine;

namespace Game.Shop
{
    [System.Serializable]
    public class ShopItemOutput
    {
        public float weight;
        public MergeItemSO mergeItem;
    }
    
    
    
    [CreateAssetMenu(menuName = "SO/Shop/" + nameof(ShopItem), fileName = nameof(ShopItem), order = 9)]
    public class ShopItem : ScriptableObject, IShopItem
    {
        [SerializeField] private string _itemId;
        [SerializeField] private float _cost;
        [SerializeField] private int _itemLevel;

        [Space(10)] 
        [SerializeField] private List<ShopItemOutput> _shopItemOutputs;

        public string ItemId => _itemId;
        public float Cost => _cost;
        public int ItemLevel => _itemLevel;

        public IList<ShopItemOutput> Outputs => _shopItemOutputs;


    }
}