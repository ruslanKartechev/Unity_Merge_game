using Common;
using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Common.SlowMotion;
using Game.Hunting;
using Game.Hunting.Hunters;
using Game.Hunting.UI;
using Game.Merging;
using Game.Shop;
using Game.UI.Merging;

namespace Game
{
    public static class GC 
    {
        public static IPlayerData PlayerData { get; set; }
        public static IDataSaver DataSaver { get; set; }
        public static ISceneSwitcher SceneSwitcher { get; set; }
        public static ILevelManager LevelManager { get; set; }
        public static IHuntersRepository HuntersRepository { get; set; }
        public static IActiveGroupSO ActiveGroupSO { get; set; }
        public static ILevelRepository LevelRepository { get; set; }
        
        public static IMergeItemViews ItemViews { get; set; }
        public static MergeItemsStashSO ItemsStash { get; set; }
        public static IMergeTable MergeTable { get; set; }
        public static IShopItems ShopItems { get; set; }
        public static IShopItemsViews ShopItemsViews { get; set; }
        public static ISlowMotionManager SlowMotion { get; set; }
        public static IPlayerInput Input { get; set; }
        public static IShopSettingsRepository ShopSettingsRepository { get; set; }
        public static ParticlesRepository ParticlesRepository { get; set; }
        
        public static UIManager UIManager { get; set; }
        public static HunterSettingsProvider HunterSettingsProvider { get; set; }
    }
}