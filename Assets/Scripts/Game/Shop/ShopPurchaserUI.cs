using Game.Merging;
using Game.Shop;
using Game.UI;
using Game.UI.Shop;
using UnityEngine;

namespace Game.Shop
{
    public class ShopPurchaserUI : MonoBehaviour, IShopPurchaser
    {
        [SerializeField] private PurchasedItemDisplay _purchasedItemDisplay;
        
        public bool Purchase(IShopItem shopItem, out MergeItem mergeItem)
        {
            Debug.Log("Purchase UI clicked");
            var success = ShopItemPurchaser.Purchase(shopItem, out mergeItem);
            if (success)
            {
                UIC.UpdateMoney();
            }
            return success;
        }
    }
}