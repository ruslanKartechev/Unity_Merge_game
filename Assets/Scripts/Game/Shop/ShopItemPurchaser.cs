using Game.Merging;
using NSubstitute.ReturnsExtensions;
using UnityEngine;
using Utils;

namespace Game.Shop
{
    public static class ShopItemPurchaser
    {
        public static bool Purchase(IShopItem shopItem, out MergeItem mergeItem)
        {
            if (!SubMoney(shopItem.Cost))
            {
                mergeItem = null;
                return false;
            }
            
            var settings = GC.ShopSettingsRepository.GetSettings(GC.PlayerData.ShopPurchaseCount);
            var msg = $"Level {GC.PlayerData.LevelTotal}";
            if (shopItem.ItemLevel == 0 
                && settings != null 
                && settings.OutputItem != null)
            {
                mergeItem = new MergeItem(settings.OutputItem);
                msg += $" || item: {settings.OutputItem.item_id}";
            }
            else
            {
                mergeItem = GetRandomMergeItem(shopItem);
                msg += " || item: RANDOM";
            }
            CLog.LogWHeader("Shop",msg, "g", "w");
            CLog.LogWHeader("Shop",$"Purchased Item {mergeItem.item_id}, Level {mergeItem.level}. Cost: {shopItem.Cost}", "g", "w");
            GC.ItemsStash.Stash.AddItem(mergeItem);
            
            GC.PlayerData.ShopPurchaseCount++;
            return true;
        }

        private static bool SubMoney(float cost)
        {
            var money = GC.PlayerData.Money;
            if (money < cost)
            {
                CLog.LogWHeader("ShopPurchaser", $"Not enough money to buy shop item. Cost: {cost}", "r", "w");
                return false;
            }
            money -= cost;
            GC.PlayerData.Money = money;
            return true;
        }

        private static MergeItem GetRandomMergeItem(IShopItem item)
        {
            float total = 0;
            foreach (var w in item.Outputs)
                total += w.weight;
            var random = UnityEngine.Random.Range(0, total);
            for (var i = 0; i < item.Outputs.Count; i++)
            {
                random -= item.Outputs[i].weight;
                if (random < 0)
                    return new MergeItem(item.Outputs[i].mergeItem.Item);
            }
            
            return null;
        }
    }
}