using System.Collections.Generic;
using Common.Saving;
using Game.Merging;
using UnityEngine;

namespace Game.Saving
{
    [System.Serializable]
    public class SavedData : ISavedData
    {
        [SerializeField] private PlayerData _playerData;
        [Space(10)]
        [SerializeField] private ActiveGroup _activeGroup;
        [Space(10)]
        [SerializeField] private MergeItemsStash _stash;
        [SerializeField] private List<SuperEggSaveData> _superEggSaveData;

        public IPlayerData PlayerData => _playerData;
        public IActiveGroup ActiveGroup => _activeGroup;
        public MergeItemsStash ItemsStash => _stash;
        public IList<SuperEggSaveData> SuperEggsData => _superEggSaveData;

        public SavedData()
        {
            _playerData = new PlayerData();
        }

        public SavedData(IPlayerData playerData)
        {
            _playerData = new PlayerData(playerData);
        }

        public SavedData(IPlayerData playerData, ActiveGroup activeGroup, MergeItemsStash stash, List<SuperEggSaveData> superEggSaveData)
        {
            _playerData = new PlayerData(playerData);
            _activeGroup = activeGroup;
            _stash = stash;
            _superEggSaveData = superEggSaveData;
        }
    }
}