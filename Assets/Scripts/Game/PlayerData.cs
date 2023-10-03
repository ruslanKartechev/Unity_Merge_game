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
        [NonSerialized] private int _environmentIndex;
        [NonSerialized] private int _levelsTotal;
        [NonSerialized] private bool _tutorPlayedAttack;
        [NonSerialized] private bool _tutorPlayedMerge;
        
        
        public float Money
        {
            get => _money;
            set => _money = value;
        }

        public float Crystal
        {
            get => _crystals;
            set => _crystals = value;
        }

        public float Crystals
        {
            get => _crystals;
            set
            {
                var prev = _crystals;
                _crystals = value; 
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
        
        public bool TutorPlayed_Attack
        {
            get => _tutorPlayedAttack;
            set => _tutorPlayedAttack = value;
        }

        public bool TutorPlayed_Merge
        {
            get => _tutorPlayedMerge;
            set => _tutorPlayedMerge = value;
        }

        /// <summary>
        /// ////////////////////!!!!!!!!!!!!!!!!!!! IMPLEMENT
        /// </summary>
        public int CurrentEnvironmentIndex { get; set; }
    }
}