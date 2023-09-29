
using Game.Merging;

namespace Game.Shop
{
    public interface IShopPurchaser
    {
        bool Purchase(IShopItem shopItem, out MergeItem mergeItem);
    }
}