using Common.Saving;
using Game.Merging;

namespace Game.Saving
{
    [System.Serializable]
    public class SavedData : ISavedData
    {
        private float _money;
        private int _levelIndex;
        private int _levelTotal;
        public ActiveGroup gridData;

        public float Money() => _money;
        public int LevelIndex() => _levelIndex;
        public int LevelTotal() => _levelTotal;
        public IActiveGroup MergeGridData() => gridData;

        public SavedData() { }

        public SavedData(IPlayerData playerData)
        {
            _money = playerData.Money;
            _levelIndex = playerData.LevelIndex;
            _levelTotal = playerData.LevelTotal;
        }

    }
}