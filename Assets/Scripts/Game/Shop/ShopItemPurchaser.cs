using Game.Merging;
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
            mergeItem = GetMergeItem(shopItem);
            GC.ItemsStash.Stash.AddItem(mergeItem);
            Debug.Log($"Purchased Item {mergeItem.item_id}, Level {mergeItem.level}. Cost: {cost}");
            return true;
        }

        private static MergeItem GetMergeItem(IShopItem item)
        {
            float total = 0;
            foreach (var w in item.Outputs)
                total += w.weight;
            var random = UnityEngine.Random.Range(0, total);
            Debug.Log($"Total {total}, random: {random}");
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