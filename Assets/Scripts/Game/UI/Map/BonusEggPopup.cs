using Common.UIPop;
using Game.UI.Hunting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Game.Merging;
using Game.UI.Merging;
using GC = Game.Core.GC;

namespace Game.UI.Map
{
    public class BonusEggPopup : MonoBehaviour
    {
        [SerializeField] private float _hideDuration;
        [SerializeField] private Button _hideButton;
        [SerializeField] private ScalePopup _popup;
        [SerializeField] private PopAnimator _popAnimator;
        [SerializeField] private MergeItemUILevel _itemLevel;
        [SerializeField] private Image _icon;

        public Action OnHidden { get; set; }

        public void ShowItem(MergeItem item, Action onHidden)
        {
            OnHidden = onHidden;
            gameObject.SetActive(true);
            _popup.PopUp(() => {});
            _popAnimator.HideAndPlay();
            _icon.sprite = GC.ItemViews.GetIcon(item.item_id);
            _itemLevel.SetLevel(item.level + 1);
            _itemLevel.Show();
            _hideButton.onClick.AddListener(Hide);
            StartCoroutine(CountdownToHide());
        }

        private void Hide()
        {
            StopAllCoroutines();
            _popup.PopDown(() =>
            {
                gameObject.SetActive(false);
                OnHidden.Invoke();
            });
        }

        private IEnumerator CountdownToHide()
        {
            yield return new WaitForSeconds(_hideDuration);
            Hide();
        }
        

        public void Hide(bool animated, Action onEnd = null)
        {
            if (animated)
            {
                _popup.PopDown(onEnd);
            }
            else
            {
                gameObject.SetActive(false);
                onEnd?.Invoke();
            }
        }
        
        
    }
}