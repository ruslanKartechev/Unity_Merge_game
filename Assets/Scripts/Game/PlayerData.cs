using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/" + nameof(PlayerData), fileName = nameof(PlayerData), order = 0)]
    public class PlayerData : ScriptableObject, IPlayerData
    {
        [NonSerialized] private float _money;
        [NonSerialized] private int _levelIndex;
        [NonSerialized] private int _levelsTotal;

        public event Action<float, float> OnMoneyUpdated;

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