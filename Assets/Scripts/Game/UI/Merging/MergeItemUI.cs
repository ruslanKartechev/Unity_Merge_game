using DG.Tweening;
using Game.Merging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeItemUI : MonoBehaviour, IMergeItemUI
    {
        private const float PunchScale = .04f;
        private const float PunchScaleTime = .3f;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private Image _fide;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private MergeItemUILevel _levelUI;
        [SerializeField] private Image _darkening;
        private MergeItem _item;

        public MergeItem Item
        {
            get => _item;
            set => _item = value;
        }

        [ContextMenu("ShowEmpty()")]
        public void SetEmpty()
        {
            // _levelUI.Hide();
            // _icon.enabled = false;
            // _nameText.enabled = false;
            Item = null;
            gameObject.SetActive(false);
        }

        [ContextMenu("ShowItemData()")]
        public void ShowItemView()
        {
            _levelUI.Show();
            _icon.enabled = true;
            _icon.sprite = GC.ItemViews.GetIcon(_item.item_id);
            _nameText.text = GC.ItemViews.GetDescription(_item.item_id).ItemName;
            _nameText.enabled = true;
            _levelUI.SetLevel(_item.level + 1);
            gameObject.SetActive(true);
        }

        public void SetMerged(MergeItem item)
        {
            PlayMerged();
            // _frameHighlighter.Highlight();
            Item = item;
            ShowItemView();
        }
        
        public void PlayMerged()
        {
            _levelUI.PlayScale();   
        }

        public void SetDarkened(bool darkened)
        {
            _darkening.enabled = darkened;
        }

        public void PlayItemSet()
        {
            transform.localScale = Vector3.one;
            transform.DOPunchScale(Vector3.one * PunchScale, PunchScaleTime);
        }

        public void SetItemAndLookEmpty(MergeItem item)
        {
            SetEmpty();
            Item = item;
        }

        public void SetBackground(Sprite icon, Sprite fide)
        {
            _background.sprite = icon;
            _fide.sprite = fide;
        }
    }
}