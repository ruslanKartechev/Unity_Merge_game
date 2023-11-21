using System;
using Common;
using Common.Utils;
using Game.UI;
using Game.UI.Merging;
using Game.UI.Shop;
using UnityEngine;

namespace Game.Merging
{
    public class ShopTutorial : Tutorial
    {
        [SerializeField] private TutorButtonsBlocker _tutorButtonsBlocker;
        [Header("Enter Shop")]
        [SerializeField] private MergeCanvasSwitcher _switcher;
        [SerializeField] private Transform _enterShopSpotlightPoint;
        [SerializeField] private float _enterShopSpotlightSize = 300;
        [Header("Click egg")]
        [SerializeField] private Transform _eggHandPoint;
        [SerializeField] private Transform _eggSpotlightPoint;
        [SerializeField] private float _buyItemSpotlightSize = 600;
        [SerializeField] private PurchasedItemDisplay _shopItemDisplay;
        [Header("Shop exit")]
        [SerializeField] private float _exitShopSpotlightSize;
        [SerializeField] private Transform _exitShopSpotlightPoint;
        [SerializeField] private Transform _exitShopHandPoint;


        public override void BeginTutorial(Action onCompleted)
        {
            _spotlight2.HideSelf();
            _tutorBlock.SetActive(true);
            _onCompleted = onCompleted;
            _buttonsBlocker.OnlyShop();
            StartCoroutine(SkipFrames(_waitFramesCount, ShowEnterShopTutor));
        }
        
        private void ShowEnterShopTutor()
        {
            _tutorButtonsBlocker.BlockToEnterShop();
            _tutorBlock.gameObject.SetActive(true);
            _spotlight1.Show();
            _spotlight1.SetSize(_enterShopSpotlightSize);
            Hand.ShowClickingAt(_enterShopSpotlightPoint.position);
            _spotlight1.SetPosition(_enterShopSpotlightPoint.position);
            _switcher.OnShop += DelayedShopTutor;
        }

        private void DelayedShopTutor()
        {
            _switcher.OnShop -= DelayedShopTutor;
            StartCoroutine(SkipFrames(_waitFramesCount, ShowShopTutor));
        }
        
        private void ShowShopTutor()
        {
            _tutorButtonsBlocker.BlockToBuyInShop();
            _switcher.OnShop -= ShowShopTutor;
            _spotlight1.SetSize(_buyItemSpotlightSize);
            _spotlight1.SetPosition(_eggSpotlightPoint.position);
            _spotlight1.Show();
            Hand.ShowClickingAt(_eggHandPoint.position);
            _shopItemDisplay.OnDisplayStarted += HideHighlight;
            _shopItemDisplay.OnDisplayEnded += ShowExitShopTutor;
        }

        private void HideHighlight()
        {
            _shopItemDisplay.OnDisplayStarted -= HideHighlight;
            _spotlight1.HideAll();
            Hand.Hide();   
        }

        private void ShowExitShopTutor()
        {
            _tutorButtonsBlocker.BlockToExitInShop();
            _shopItemDisplay.OnDisplayEnded -= ShowExitShopTutor;
            _buttonsBlocker.OnlyExitShop();
            _spotlight1.SetSize(_exitShopSpotlightSize);
            _spotlight1.SetPosition(_exitShopSpotlightPoint.position);
            _spotlight1.Show();
            Hand.ShowClickingAt(_exitShopHandPoint.position);
            _switcher.OnMain += OnMain;
        }

        private void OnMain()
        {
            CLog.LogWHeader("EggPurchaseTutor", "Tutorial ended", "b", "w");
            _switcher.OnMain -= OnMain;
            _buttonsBlocker.All();
            _onCompleted.Invoke();
        }



    }
}