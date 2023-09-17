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
    
    
    [System.Serializable]
    public class ShopItem : IShopItem
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