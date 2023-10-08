namespace Game.Shop
{
    public interface IShopSettingsRepository
    {
        IShopSettings GetSettings(int level);
        
    }
}