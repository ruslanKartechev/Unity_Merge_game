using System.Collections.Generic;
using Common.UIEffects;
using Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Shop
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private RawImage _icon;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private List<ChancesDisplay> _chancesDisplays;
        [Space(10)]
        [SerializeField] private Button _button;
        [SerializeField] private ButtonClickEffect _clickEffect;
        private IShopItem _shopItem;
        private GameObject _itemInstance;
        private IShopEgg _egg;
        
        public IShopPurchaser Purchaser { get; set; }
        public PurchasedItemDisplay PurchasedItemDisplay { get; set; }

        public void ActivateButton() => _button.interactable = true;
        public void DeactivateButton() => _button.interactable = false;
        
        
        public void SetItem(IShopItem shopItem)
        {
            _shopItem = shopItem;
            
            var view = GC.ShopItemsViews.GetView(shopItem.ItemId);
            if(_itemInstance != null)
                Destroy(_itemInstance);
            _itemInstance = Instantiate(view.Prefab);
            _itemInstance.transform.position = transform.position;
            _egg = _itemInstance.GetComponent<IShopEgg>();
            
            _label.text = view.DisplayedName;
            _icon.texture = view.RenderTexture;
            _costText.text = $"{shopItem.Cost}";
            SetupChances(shopItem);
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(Purchase);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetupChances(IShopItem shopItem)
        {
            _chancesDisplays[0].ShowChance("Land", shopItem.Outputs[0].weight);
            _chancesDisplays[1].ShowChance("Water", shopItem.Outputs[1].weight);
            _chancesDisplays[2].ShowChance("Air", shopItem.Outputs[2].weight);
        }

        private void Purchase()
        {
            _clickEffect.Play();
            _icon.enabled = false;
            if (Purchaser.Purchase(_shopItem, out var mergeItem))
            {
                PurchasedItemDisplay.ShowItemPurchased(mergeItem.item_id, _shopItem, _egg, _icon.texture, () =>
                {
                    _egg.Reset();
                    PurchasedItemDisplay.HideNow();
                    _icon.enabled = true;
                });
            }
        }

        
        
        [System.Serializable]
        public class ChancesDisplay
        {
            [SerializeField] private TextMeshProUGUI _name;
            [SerializeField] private TextMeshProUGUI _percent;

            public void ShowChance(string name, float percent)
            {
                _name.text = name;
                _percent.text = $"{percent}%";
            }
        }
    }
}