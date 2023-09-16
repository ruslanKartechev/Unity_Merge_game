using Game.Shop;
using Game.UI.Elements;
using UnityEngine;

namespace Game.Shop
{
    public class ShopPurchaser : MonoBehaviour, IShopPurchaser
    {
        [SerializeField] private MoneyDisplayUI _moneyDisplay;
        
        public bool Purchase(IShopItem item)
        {
            var cost = item.Cost;
            var money = GC.PlayerData.Money;
            if (money < cost)
            {
                Debug.Log($"Not enough money to buy shop item. Cost: {cost}");
                return false;
            }
            money -= cost;
            GC.PlayerData.Money = money;
            _moneyDisplay.UpdateCount();
            return true;
        }
    }
}