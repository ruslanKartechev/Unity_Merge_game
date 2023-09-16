using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ShopItemsViews), fileName = nameof(ShopItemsViews), order = 5)]
    public class ShopItemsViews : ScriptableObject, IShopItemsViews
    {
        [SerializeField] private List<Data> _data;
        [NonSerialized] private Dictionary<string, Data> _table = new Dictionary<string, Data>(); 

        
        
        [System.Serializable]
        public class Data : IShopItemView
        {
            public string id;
            [SerializeField] private Sprite _icon;
            [SerializeField] private Color _backgroundColor;
            [SerializeField] private string _label;
            
            public Sprite Sprite => _icon;
            public Color BackgroundColor => _backgroundColor;
            public string DisplayedName => _label;
        }
        
       
        public void Init()
        {
            _table = new Dictionary<string, Data>(_data.Count);
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