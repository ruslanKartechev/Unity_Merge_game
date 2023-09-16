namespace Game.Shop
{
    public interface IShopItem
    {
        string ItemId { get; }
        float Cost { get; }
        float Chance1 { get; }
        float Chance2 { get; }
        float Chance3 { get; }
        float Chance4 { get; }
    }
}