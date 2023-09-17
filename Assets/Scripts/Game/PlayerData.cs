using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/" + nameof(PlayerData), fileName = nameof(PlayerData), order = 0)]
    public class PlayerData : ScriptableObject, IPlayerData
    {
        [NonSerialized] private float _money;
        [NonSerialized] private float _crystals;
        [NonSerialized] private int _levelIndex;
        [NonSerialized] private int _levelsTotal;

        public event Action<float, float> OnMoneyUpdated;
        public event Action<float, float> OnCrystalsUpdated;

        public float Money
        {
            get => _money;
            set
            {
                var prev = _money;
                _money = value; 
                OnMoneyUpdated?.Invoke(prev, _money);
            }
        }
        
        public float Crystals
        {
            get => _crystals;
            set
            {
                var prev = _crystals;
                _crystals = value; 
                OnCrystalsUpdated?.Invoke(prev, _crystals);
            }
        }

        public int LevelIndex
        {
            get => _levelIndex;
            set => _levelIndex = value;
        }

        public int LevelTotal
        {
            get => _levelsTotal;
            set => _levelsTotal = value;
        }
    }
}