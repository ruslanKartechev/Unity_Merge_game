using DG.Tweening;
using Game.Merging;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeItemUI : MonoBehaviour, IMergeItemUI
    {
        private const float PunchScale = .04f;
        private const float PunchScaleTime = .3f;

        [SerializeField] private Image _icon;
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
            _levelUI.Hide();
            _icon.enabled = false;
            Item = null;
        }

        [ContextMenu("ShowItemData()")]
        public void ShowItemData()
        {
            _levelUI.Show();
            _icon.enabled = true;
            _icon.sprite = GC.ItemViewRepository.GetIcon(_item.item_id);
            _levelUI.SetLevel(_item.level + 1);
        }

        public void SetMerged(MergeItem item)
        {
            PlayMerged();
            Item = item;
            ShowItemData();
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

        public Sprite CurrentIcon => _icon.sprite;
    }
}