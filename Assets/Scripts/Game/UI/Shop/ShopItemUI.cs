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
        [SerializeField] private Image _background;
        [SerializeField] private Image _backgroundFrame;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private List<ChancesDisplay> _chancesDisplays;
        [Space(10)]
        [SerializeField] private Button _button;
        [SerializeField] private ButtonClickEffect _clickEffect;
        private IShopItem _item;
        public IShopPurchaser Purchaser { get; set; }
        private GameObject _itemInstance;
        
        public void SetItem(IShopItem shopItem)
        {
            _item = shopItem;
            
            var view = GC.ShopItemsViews.GetView(shopItem.ItemId);
            if(_itemInstance != null)
                Destroy(_itemInstance);
            _itemInstance = Instantiate(view.Prefab);
            _itemInstance.transform.position = transform.position;
            
            _label.text = view.DisplayedName;
            _icon.texture = view.RenderTexture;
            _background.color = view.BackgroundColor;
            _backgroundFrame.color = _background.color * .6f;
            _costText.text = $"{shopItem.Cost}";
            SetupChances(shopItem);
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(Purchase);
        }

        private void SetupChances(IShopItem shopItem)
        {
            _chancesDisplays[0].ShowChance("Land", shopItem.Outputs[0].weight);
            _chancesDisplays[1].ShowChance("Water", shopItem.Outputs[1].weight);
            _chancesDisplays[2].ShowChance("Air", shopItem.Outputs[2].weight);
        }

        private void Purchase()
        {
            Purchaser.Purchase(_item);
            _clickEffect.Play();
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