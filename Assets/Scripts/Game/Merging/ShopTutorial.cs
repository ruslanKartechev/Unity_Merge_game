using System;
using Common;
using Game.UI.Merging;
using Game.UI.Shop;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class ShopTutorial : Tutorial
    {
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
        private Action _onCompleted;
        

        public void PlayTutor(Action onCompleted)
        {
            _tutorBlock.SetActive(true);
            _onCompleted = onCompleted;
            StartCoroutine(SkipFrames(_waitFramesCount, ShowEnterShopTutor));
        }
        
        private void ShowEnterShopTutor()
        {
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
            _spotlight1.Hide();
            Hand.Hide();   
        }

        private void ShowExitShopTutor()
        {
            _shopItemDisplay.OnDisplayEnded -= ShowExitShopTutor;

            _spotlight1.SetSize(_exitShopSpotlightSize);
            _spotlight1.SetPosition(_exitShopSpotlightPoint.position);
            _spotlight1.Show();
            Hand.ShowClickingAt(_exitShopHandPoint.position);
            _switcher.OnMain += OnMain;
        }

        private void OnMain()
        {
            CLog.LogWHeader("EggPurchaseTutor", "Tutorial ended", "b", "w");
            _onCompleted.Invoke();
        }



    }
}