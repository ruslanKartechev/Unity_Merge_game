using System;
using Game.UI;
using Game.UI.Elements;
using Game.UI.Hunting;
using UnityEngine;
using GC = Game.Core.GC;

namespace Creatives
{
    [DefaultExecutionOrder(-20)]
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
        [SerializeField] private FlyingMoney _flyingMoney;
        private float _money;
        private int _kills;

        public IFlyingMoney FlyingMoney => _flyingMoney;
        
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

            UIC.Money = _moneyUI;
            _money = startMoney;
            _kills = killsStart;
            GC.PlayerData.Money = _money;
            _moneyUI.UpdateCount();
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