using System;
using Game.UI.Elements;
using Game.UI.Hunting;
using UnityEngine;

namespace Creatives
{
    [DefaultExecutionOrder(-200)]
    public class CreosUI : MonoBehaviour
    {
        public static CreosUI Instance { get; set; }

        public bool showLevel;
        public int startLevel;
        [Space(10)]
        [Header("Start stats")]
        public float startMoney;
        public int killsMax;
        public int killsStart;
        [Space(10)] 
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private MoneyUI _moneyUI;
        [SerializeField] private KillCountDisplayUI _killCount;
        private float _money;
        private int _kills;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (showLevel)
            {
                _levelDisplay.gameObject.SetActive(true);
                _levelDisplay.SetLevel(startLevel);
            }
            else
            {
                _levelDisplay.gameObject.SetActive(false);
            }
            _money = startMoney;
            _kills = killsStart;
            _moneyUI.UpdateCount();
            _moneyUI.SetCount(_money, false);
        }

        public void AddMoney(float money)
        {
            _money += money;
            _moneyUI.SetCount(_money,true);       
                
        }

        public void AddKill(int added)
        {
            _kills += added;
            _killCount.SetKillCount(_kills, killsMax);
        }
        
        
    }
}