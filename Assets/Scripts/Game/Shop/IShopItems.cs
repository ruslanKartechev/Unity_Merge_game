namespace Game.Shop
{
    public interface IShopItems
    {
        int Count { get; }
        IShopItem GetItem(int index);
    }
}