using System;
using Common;
using Game.Merging;
using Game.UI.Elements;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergingPage : MonoBehaviour, IMergingPage
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private MoneyDisplayUI _moneyDisplay;
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;

        private void Start()
        {
            _mergeManager.SetUI(this);
            LoadingCurtain.Open(() => {});
        }

        private void OnEnable()
        {
            Show();
        }

        
        public void Show()
        {
            _classesSwitcher.ShowDefault();
            UpdateMoney();
            UpdateCrystals();
            UpdateLevel();
        }
        
        public void UpdateMoney()
        {
            _moneyDisplay.UpdateCount();
        }

        public void UpdateCrystals()
        { }

        public void UpdateLevel()
        {
            _levelDisplay.SetLevel(GC.PlayerData.LevelTotal + 1);
        }
        
    }
}