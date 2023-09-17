using System;
using System.Collections.Generic;
using Game.Shop;
using Game.UI.Merging;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private MergingPage _merging;
        [SerializeField] private Button _closeButton;
        [SerializeField] private ShopManager _shopManager;
        [SerializeField] private ShopPurchaserUI _shopPurchaserUI;
        [SerializeField] private PurchasedItemDisplay _purchasedItemDisplay;
        [SerializeField] private List<ShopItemUI> _shopItemUis;

        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePage);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(ClosePage);
        }

        public void Init()
        {
            _purchasedItemDisplay.HideNow();      
            _shopManager.Init();
            SetItemUIs();
        }

        private void ClosePage()
        {
            Debug.Log("Close shop button");     
            _merging.Show();
        }

        private void SetItemUIs()
        {
            var items = GC.ShopItems;
            var count = items.Count;
            for (var i = 0; i < count; i++)
            {
                var data = items.GetItem(i);
                _shopItemUis[i].Purchaser = _shopPurchaserUI;
                _shopItemUis[i].SetItem(data);
            }
        }
    }
}