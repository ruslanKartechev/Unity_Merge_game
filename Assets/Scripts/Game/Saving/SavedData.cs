using System;
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
        [SerializeField] private int _levelIndex;
        [SerializeField] private int _levelTotal;
        [SerializeField] private int _environmentIndex;
        
        public ActiveGroup gridData;

        public float Money() => _money;
        public float Crystal() => _crystals;
        public int LevelIndex() => _levelIndex;
        public int LevelTotal() => _levelTotal;
        public int EnvironmentIndex() => _environmentIndex;
        public IActiveGroup MergeGridData() => gridData;

        
        
        public SavedData() { }

        public SavedData(IPlayerData playerData)
        {
            _money = playerData.Money;
            _levelIndex = playerData.LevelIndex;
            _levelTotal = playerData.LevelTotal;
            _environmentIndex = playerData.EnvironmentIndex;
        }

    }
}