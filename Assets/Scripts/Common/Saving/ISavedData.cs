using Game.Merging;

namespace Common.Saving
{
    public interface ISavedData
    {
        float Money();
        int LevelIndex();
        int LevelTotal();
        IActiveGroup MergeGridData();
    }
}