using System.Collections.Generic;
using Common.Saving;
using Game.Merging;
using UnityEngine;

namespace Game.Saving
{
    [System.Serializable]
    public class SavedData : ISavedData
    {
        [SerializeField] private float _money;
        [SerializeField] private float _crystals;
        [Space(10)]
        [SerializeField] private int _levelIndex;
        [SerializeField] private int _levelTotal;
        [Space(10)]
        [SerializeField] private ActiveGroup _activeGroup;
        [Space(10)]
        [SerializeField] private MergeItemsStash _stash;
        [SerializeField] private List<SuperEggSaveData> _superEggSaveData;

        public float Money() => _money;
        public float Crystal() => _crystals;
        public int LevelIndex() => _levelIndex;
        public int LevelTotal() => _levelTotal;
        public IActiveGroup ActiveGroup => _activeGroup;
        public MergeItemsStash ItemsStash => _stash;
        public IList<SuperEggSaveData> SuperEggsData => _superEggSaveData;

        public SavedData()
        {}

        public SavedData(IPlayerData playerData)
        {
            _money = playerData.Money;
            _crystals = playerData.Crystal;
            _levelIndex = playerData.LevelIndex;
            _levelTotal = playerData.LevelTotal;
        }

        public SavedData(IPlayerData playerData, ActiveGroup activeGroup, MergeItemsStash stash, List<SuperEggSaveData> superEggSaveData)
        {
            _money = playerData.Money;
            _crystals = playerData.Crystal;
            _levelIndex = playerData.LevelIndex;
            _levelTotal = playerData.LevelTotal;

            _activeGroup = activeGroup;
            _stash = stash;
            
            _superEggSaveData = superEggSaveData;
        }
    }
}