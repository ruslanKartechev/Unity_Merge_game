using System;
using System.Collections;
using Common;
using Common.UIEffects;
using DG.Tweening;
using Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GC = Game.Core.GC;

namespace Game.UI.Shop
{
    public class PurchasedItemDisplay : MonoBehaviour
    {
        public event Action OnDisplayStarted;
        public event Action OnDisplayEnded;
        
        [SerializeField] private GameObject _block;
        [SerializeField] private GameObject _stars;
        [SerializeField] private Shining _shining;
        [SerializeField] private ScalePulser _itemIconPulser;
        [Space(5)] 
        [SerializeField] private Button _closeButton;
        [Space(5)]
        [SerializeField] private RectTransform _eggTr;
        [SerializeField] private RectTransform _iconTr;
        [SerializeField] private RawImage _eggIcon;
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
        private bool _isCracked;   

        public void HideNow()
        {
            Stop();
            _block.SetActive(false);
            _darkening.SetActive(false);
            _stars.SetActive(false);
            _closeButton.gameObject.SetActive(false);
        }
        
        public void ShowItemPurchased(string mergeItemID, IShopItem shopItem, IShopEgg egg, Texture renderTexture, Action onEnd)
        {
            Stop();
            _block.SetActive(true);
            _callback = onEnd;
            _closeButton.interactable = false;
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(CloseDisplay);
            _eggIcon.texture = renderTexture;
            
            _itemIcon.sprite = GC.ItemViews.GetIcon(mergeItemID);
            _levelText.text = $"lvl {shopItem.ItemLevel + 1}";
            _working = StartCoroutine(Working(egg, GC.ItemViews.GetDescription(mergeItemID).ItemName));
            OnDisplayStarted?.Invoke();
        }

        private void CloseDisplay()
        {
            StopAllCoroutines();
            _callback?.Invoke();
            _callback = null;
            OnDisplayEnded?.Invoke();
        }

        private void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private IEnumerator Working(IShopEgg egg, string label)
        {
            _isCracked = false;
            SetCrackingState();
            egg.Crack(OnCracked);
            while (_isCracked == false)
                yield return null;
            _closeButton.gameObject.SetActive(true);
            _closeButton.interactable = true;
            // SHOULD WE HIDE IT ?
            // _eggTr.gameObject.SetActive(false);
            ShowPurchasedItemLabel(label);
            ShowPurchasedItemIcon();
            ShowPurchasedItemLevel();
            yield return new WaitForSeconds(_iconScaleDownTime);
            _itemIconPulser.Begin();
            yield return new WaitForSeconds(_displayTime);
            CloseDisplay();
        }

        private void ShowPurchasedItemIcon()
        {
            _iconTr.gameObject.SetActive(true);
            _iconTr.localScale = Vector3.one * _iconScaleUp;
            _iconTr.DOScale(Vector3.one, _iconScaleDownTime).SetEase(Ease.OutSine);
            _iconTr.localEulerAngles = new Vector3(0, -90, 0);
            _iconTr.DOLocalRotate(Vector3.zero, _iconScaleDownTime);   
        }

        private void ShowPurchasedItemLabel(string label)
        {
            _itemLable.text = label;
            _itemLable.enabled = true;
            _itemLable.transform.localScale = new Vector3(1, .2f, 1);
            _itemLable.transform.DOScaleY(1, _iconScaleDownTime).SetEase(Ease.OutBounce);   
        }

        private void ShowPurchasedItemLevel()
        {
            _levelBlock.gameObject.SetActive(true);
            _levelBlock.transform.localScale = new Vector3(1, .2f, 1);
            _levelBlock.transform.DOScaleY(1, _iconScaleDownTime).SetEase(Ease.OutBounce);   
        }
        
        private void SetCrackingState()
        {
            _iconTr.gameObject.SetActive(false);
            _stars.SetActive(true);
            _itemLable.enabled = true;
            _itemLable.text = "???";
            _darkening.SetActive(true);
            // _shining.Begin();
            _eggTr.gameObject.SetActive(true);
            _levelBlock.gameObject.SetActive(false);
        }

        private void OnCracked()
        {
            _isCracked = true;
        }
    }
}