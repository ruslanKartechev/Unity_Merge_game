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
            var cost = shopItem.Cost;
            var money = GC.PlayerData.Money;
            if (money < cost)
            {
                CLog.LogWHeader("ShopPurchaser", $"Not enough money to buy shop item. Cost: {cost}", "w", "r");
                mergeItem = null;
                return false;
            }
            money -= cost;
            GC.PlayerData.Money = money;
            
            var settings = GC.ShopSettingsRepository.GetSettings(GC.PlayerData.LevelTotal);
            var logmsg = $"Level {GC.PlayerData.LevelTotal}";
            if (settings != null && settings.OutputItem != null)
            {
                mergeItem = new MergeItem(settings.OutputItem);
                logmsg += $" || item: {settings.OutputItem.item_id}";
            }
            else
            {
                mergeItem = GetRandomMergeItem(shopItem);
                logmsg += " || item: RANDOM";
            }
            
            CLog.LogWHeader("Shop",logmsg, "g", "w");
            CLog.LogWHeader("Shop",$"Purchased Item {mergeItem.item_id}, Level {mergeItem.level}. Cost: {cost}", "g", "w");
            GC.ItemsStash.Stash.AddItem(mergeItem);
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