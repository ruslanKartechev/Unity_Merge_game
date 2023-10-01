using System;
using System.Collections.Generic;
using Game.Shop;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ShopPurchaserUI _shopPurchaserUI;
        [SerializeField] private PurchasedItemDisplay _purchasedItemDisplay;
        [SerializeField] private List<ShopItemUI> _shopItemUis;
        private Action _onClosed;
        
        
        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePage);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(ClosePage);
        }

        public void Show(Action onClosed)
        {
            CLog.LogWHeader("ShopUI", $"Show UI, onClosed ?? {onClosed == null}", "b", "w");
            gameObject.SetActive(true);
            _onClosed = onClosed;
            _purchasedItemDisplay.HideNow();      
            SetItemUIs();
        }

        private void ClosePage()
        {
            CLog.LogWHeader("ShopUI", "Close Button pressed", "b", "w");
            _onClosed?.Invoke();
        }

        private void SetItemUIs()
        {
            var items = GC.ShopItems;
            var count = items.Count;
            for (var i = 0; i < count; i++)
            {
                var data = items.GetItem(i);
                _shopItemUis[i].Purchaser = _shopPurchaserUI;
                _shopItemUis[i].PurchasedItemDisplay = _purchasedItemDisplay;
                _shopItemUis[i].SetItem(data);
            }
        }
    }
}