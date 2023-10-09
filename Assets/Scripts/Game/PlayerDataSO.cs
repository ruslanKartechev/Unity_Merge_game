﻿using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/" + nameof(PlayerDataSO), fileName = nameof(PlayerDataSO), order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        [SerializeField] private PlayerData _playerData;

        public PlayerData Data
        {
            get => _playerData;
            set
            {
                _playerData = value;
            }
        }
        
    }
}