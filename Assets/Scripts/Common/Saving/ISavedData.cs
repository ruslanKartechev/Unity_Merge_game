using Game.Merging;

namespace Common.Saving
{
    public interface ISavedData
    {
        float Money();
        float Crystal();
        
        int LevelIndex();
        int LevelTotal();
        int EnvironmentIndex();
        IActiveGroup MergeGridData();
    }
}