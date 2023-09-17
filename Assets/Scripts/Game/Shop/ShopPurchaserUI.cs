using Game.Shop;
using Game.UI.Elements;
using Game.UI.Shop;
using UnityEngine;

namespace Game.Shop
{
    public class ShopPurchaserUI : MonoBehaviour, IShopPurchaser
    {
        [SerializeField] private MoneyDisplayUI _moneyDisplay;
        [SerializeField] private PurchasedItemDisplay _purchasedItemDisplay;
        
        public bool Purchase(IShopItem shopItem)
        {
            Debug.Log("Purchase UI clicked");
            var success = ShopItemPurchaser.Purchase(shopItem, out var mergeItem);
            if (success)
            {
                _moneyDisplay.UpdateCount();
                _purchasedItemDisplay.DisplayItem(mergeItem, shopItem, OnItemDisplayed);
            }
            return success;
        }

        private void OnItemDisplayed()
        {
            Debug.Log($"Item displayed end");
            _purchasedItemDisplay.HideNow();
        }
    }
}