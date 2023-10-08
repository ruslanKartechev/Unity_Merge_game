using Game.Merging;

namespace Game.Shop
{
    public interface IShopSettings
    {
        int MaxLevel { get; }
        MergeItem OutputItem { get; }
    }
}