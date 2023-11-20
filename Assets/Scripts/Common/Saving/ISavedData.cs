using System.Collections.Generic;
using Game;
using Game.Core;
using Game.Merging;
using Game.Merging.Interfaces;

namespace Common.Saving
{
    public interface ISavedData
    {
        IPlayerData PlayerData { get; }
        IActiveGroup ActiveGroup { get; }
        MergeItemsStash ItemsStash { get; }
        IList<SuperEggSaveData> SuperEggsData { get; }

    }
}