using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerData : IPlayerData
    {
        [SerializeField] private float _money;
        [SerializeField] private float _crystals;
        [Space(10)]
        [SerializeField] private int _levelIndex;
        [SerializeField] private int _levelsTotal;
        [Space(10)]
        [SerializeField] private bool _tutorPlayedAttack;
        [SerializeField] private bool _tutorPlayedMerge;
        [SerializeField] private bool _tutorPlayedPurchased;
        [Space(10)]
        [SerializeField] private int _environmentIndex;
        [SerializeField] private int _shopPurchaseCount;


        public PlayerData(){}
        
        public PlayerData(IPlayerData from)
        {
            _money = from.Money;
            _crystals = from.Crystal;
            _levelIndex = from.LevelIndex;
            _environmentIndex = from.CurrentEnvironmentIndex;
            _levelsTotal = from.LevelTotal;
            _tutorPlayedAttack = from.TutorPlayed_Attack;
            _tutorPlayedMerge = from.TutorPlayed_Merge;
            _tutorPlayedPurchased = from.TutorPlayed_Purchased;
            _shopPurchaseCount = from.ShopPurchaseCount;
            _environmentIndex = from.CurrentEnvironmentIndex;
        }


        private const float CheatConstMoney = 10000;
        public float Money
        {
            get => CheatConstMoney;
            set
            {
                
            }
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

        public int ShopPurchaseCount
        {
            get => _shopPurchaseCount;
            set => _shopPurchaseCount = value;
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

        public bool TutorPlayed_Purchased
        {
            get => _tutorPlayedPurchased;
            set => _tutorPlayedPurchased = value;
        }
        
        /// <summary>
        /// ////////////////////!!!!!!!!!!!!!!!!!!! IMPLEMENT
        /// </summary>
        public int CurrentEnvironmentIndex
        {
            get => _environmentIndex;
            set => _environmentIndex = value;
        }
    }
}