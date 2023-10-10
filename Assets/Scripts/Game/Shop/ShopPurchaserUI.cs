using Game.Merging;
using Game.UI;
using UnityEngine;

namespace Game.Shop
{
    public class ShopPurchaserUI : MonoBehaviour, IShopPurchaser
    {
        public bool Purchase(IShopItem shopItem, out MergeItem mergeItem)
        {
            var success = ShopItemPurchaser.Purchase(shopItem, out mergeItem);
            if (success)
                UIC.UpdateMoney();
            return success;
        }
        
    }
}