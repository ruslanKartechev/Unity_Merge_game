using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Game.Merging;
using Game.Saving;
using Game.UI.Merging;

namespace Game
{
    public static class Container 
    {
        public static IPlayerData PlayerData { get; set; }
        public static IDataSaver DataSaver { get; set; }
        public static ISceneSwitcher SceneSwitcher { get; set; }
        public static ILevelManager LevelManager { get; set; }
        public static IHuntersRepository HuntersRepository { get; set; }
        public static IMergeGridRepository MergeGridRepository { get; set; }
        public static ILevelRepository LevelRepository { get; set; }
 
    }
}