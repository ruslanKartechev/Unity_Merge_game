using UnityEngine;

namespace Game.Shop
{
    [System.Serializable]
    public class ShopItem : IShopItem
    {
        [SerializeField] private string _itemId;
        [SerializeField] private float _cost;
        [SerializeField] private float _landChance;
        [SerializeField] private float _waterChance;
        [SerializeField] private float _airChance;
        [SerializeField] private float _superChance;

        public string ItemId => _itemId;
        public float Cost => _cost;
        public float Chance1 => _landChance;
        public float Chance2 => _waterChance;
        public float Chance3 => _airChance;
        public float Chance4 => _superChance;
    }
}