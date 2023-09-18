using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ShopItemsViews), fileName = nameof(ShopItemsViews), order = 8)]
    public class ShopItemsViews : ScriptableObject, IShopItemsViews
    {
        [SerializeField] private List<ShopItemView> _data;
        [NonSerialized] private Dictionary<string, ShopItemView> _table = new Dictionary<string, ShopItemView>();

        public void Init()
        {
            _table = new Dictionary<string, ShopItemView>(_data.Count);
            foreach (var dd in _data)
                _table.Add(dd.id, dd);
        }
        
        public IShopItemView GetView(string id)
        {
            if(_table.ContainsKey(id))
                return _table[id];
            Debug.Log($"[ShopItemViews]Item with id: {id} is not found!");
            return null;   
        }
    }
}