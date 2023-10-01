using System.Collections.Generic;
using Game.Merging;

namespace Common.Saving
{
    public interface ISavedData
    {
        float Money();
        float Crystal();
        
        int LevelIndex();
        int LevelTotal();
        IActiveGroup ActiveGroup { get; }
        MergeItemsStash ItemsStash { get; }
        IList<SuperEggSaveData> SuperEggsData { get; }
    }
}