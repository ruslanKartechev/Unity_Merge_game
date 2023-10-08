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
        
        
        private void OnEnable()
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
            
            _purchasedItemDisplay.OnDisplayStarted += DeactivatePurchaseButtons;
            _purchasedItemDisplay.OnDisplayEnded += ActivatePurchaseButtons;
        }

        private void ClosePage()
        {
            CLog.LogWHeader("ShopUI", "Close Button pressed", "b", "w");
            _purchasedItemDisplay.OnDisplayStarted -= DeactivatePurchaseButtons;
            _purchasedItemDisplay.OnDisplayEnded -= ActivatePurchaseButtons;
            
            _onClosed?.Invoke();
        }

        private void SetItemUIs()
        {
            var items = GC.ShopItems;
            var count = items.Count;
 
            var settings = GetSettings();
            if (settings != null && (settings.MaxLevel < 0 || settings.MaxLevel < count))
                count = settings.MaxLevel;
            else
                Debug.Log($"ShopSettings max level == -1 OR > count");
            ShowItems(count);
        }

        private void ShowItems(int count)
        {
            var items = GC.ShopItems;
            for (var i = 0; i < count; i++)
            {
                var data = items.GetItem(i);
                _shopItemUis[i].Purchaser = _shopPurchaserUI;
                _shopItemUis[i].PurchasedItemDisplay = _purchasedItemDisplay;
                _shopItemUis[i].SetItem(data);
            }
            
            for(var i = count; i < _shopItemUis.Count; i++)
                _shopItemUis[i].Hide();
        }

        private IShopSettings GetSettings() => GC.ShopSettingsRepository.GetSettings(GC.PlayerData.LevelTotal);
        
        private void ActivatePurchaseButtons()
        {
            foreach (var itemUI in _shopItemUis)
                itemUI.ActivateButton();
        }

        private void DeactivatePurchaseButtons()
        {
            foreach (var itemUI in _shopItemUis)
                itemUI.DeactivateButton();
        }
    }
}