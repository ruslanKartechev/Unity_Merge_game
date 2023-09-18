using System.Collections.Generic;

namespace Game.Shop
{
    public interface IShopItem
    {
        string ItemId { get; }
        float Cost { get; }
        IList<ShopItemOutput> Outputs { get; }
        int ItemLevel { get; }
  
    }
}