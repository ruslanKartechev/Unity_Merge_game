using System;
using System.Collections;
using Common;
using Common.UIEffects;
using DG.Tweening;
using Game.Merging;
using Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Shop
{
    public class PurchasedItemDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _block;
        [SerializeField] private GameObject _stars;
        [SerializeField] private Shining _shining;
        [SerializeField] private ScalePulser _itemIconPulser;
        [Space(5)] 
        [SerializeField] private Button _closeButton;
        [Space(5)]
        [SerializeField] private float _shakeDuration;
        [SerializeField] private float _shakeMagnitude;
        [SerializeField] private RectTransform _eggTr;
        [SerializeField] private RectTransform _iconTr;
        [SerializeField] private Image _eggIcon;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private GameObject _darkening;
        [SerializeField] private TextMeshProUGUI _itemLable;
        [Space(10)] 
        [SerializeField] private GameObject _levelBlock;
        [SerializeField] private TextMeshProUGUI _levelText;
        [Space(10)] 
        [SerializeField] private float _iconScaleUp = 1.1f;
        [SerializeField] private float _iconScaleDownTime = .5f;
        [Space(10)] 
        [SerializeField] private float _displayTime = 1f;
        private Coroutine _working;
        private Action _callback;

        public void HideNow()
        {
            Stop();
            _block.SetActive(false);
            _darkening.SetActive(false);
            _stars.SetActive(false);
            _closeButton.gameObject.SetActive(false);
        }
        
        public void DisplayItem(MergeItem item, IShopItem shopItem, Action onEnd)
        {
            Stop();
            _block.SetActive(true);
            _callback = onEnd;
            _closeButton.interactable = false;
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(CloseDisplay);
            _working = StartCoroutine(Working(item.item_id, shopItem.ItemId, shopItem.ItemLevel));
        }

        private void CloseDisplay()
        {
            Debug.Log("Close display button on click");
            StopAllCoroutines();
            _callback?.Invoke();
        }

        private void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }
            
        private IEnumerator Working(string itemID, string shopItemId, int itemLevel)
        {
            _iconTr.gameObject.SetActive(false);
            _stars.SetActive(true);
            _itemLable.enabled = true;
            _itemLable.text = "???";
            _darkening.SetActive(true);
            _shining.Begin();
            _eggIcon.sprite = GC.ShopItemsViews.GetView(shopItemId).Sprite;
            _eggTr.gameObject.SetActive(true);
            _eggTr.DOShakeAnchorPos(_shakeDuration, _shakeMagnitude);
            _itemIconPulser.Stop();
            _levelBlock.gameObject.SetActive(false);
            
            yield return new WaitForSeconds(_shakeDuration);
            _closeButton.gameObject.SetActive(true);
            _closeButton.interactable = true;
            
            _eggTr.gameObject.SetActive(false);
            _itemIcon.sprite = GC.ItemViews.GetIcon(itemID);
            
            _iconTr.gameObject.SetActive(true);
            _iconTr.localScale = Vector3.one * _iconScaleUp;
            _iconTr.DOScale(Vector3.one, _iconScaleDownTime).SetEase(Ease.OutSine);
            _iconTr.localEulerAngles = new Vector3(0, -90, 0);
            _iconTr.DOLocalRotate(Vector3.zero, _iconScaleDownTime);

            _itemLable.enabled = true;
            _itemLable.text = GC.ItemViews.GetDescription(itemID).ItemName;
            _itemLable.transform.localScale = new Vector3(1, .2f, 1);
            _itemLable.transform.DOScaleY(1, _iconScaleDownTime).SetEase(Ease.OutBounce);

            _levelText.text = $"lvl {itemLevel + 1}";
            _levelBlock.gameObject.SetActive(true);
            _levelBlock.transform.localScale = new Vector3(1, .2f, 1);
            _levelBlock.transform.DOScaleY(1, _iconScaleDownTime).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(_iconScaleDownTime);
            _itemIconPulser.Begin();
            yield return new WaitForSeconds(_displayTime);
            _callback?.Invoke();
        }
        
    }
}