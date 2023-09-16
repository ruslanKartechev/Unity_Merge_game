namespace Game.Shop
{
    public interface IShopItemsViews
    {
        IShopItemView GetView(string id);
    }
}