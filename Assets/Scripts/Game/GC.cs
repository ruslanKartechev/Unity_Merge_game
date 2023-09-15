using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Game.Merging;
using Game.Saving;
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
        public static IActiveGroupSO GridRepository { get; set; }
        public static ILevelRepository LevelRepository { get; set; }
        
        public static IMergeItemViewRepository ItemViewRepository { get; set; }
        public static MergeItemsStashSO ItemsStash { get; set; }
        public static IMergeTable MergeTable { get; set; }
    
    }
}