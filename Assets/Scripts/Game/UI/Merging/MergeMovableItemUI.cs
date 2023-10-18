using Game.Merging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeMovableItemUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _movable;
        [SerializeField] private GameObject _block;
        [SerializeField] private Image _icon;
        private IMergeItemUI _fromCell;
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public IMergeItemUI FromCell => _fromCell;
        private bool _fromCellUsed;

        public MergeItem Item => _fromCell.Item;
        
        public void Setup(IMergeItemUI fromCell)
        {
            _fromCell = fromCell;
            _fromCell.SetDarkened(true);
            SetItem(fromCell.Item);
            _fromCellUsed = true;
        }

        public void SetupNoFromCellEffect(MergeItem item, IMergeItemUI fromCell)
        {
            _fromCell = fromCell;
            _fromCell.SetItemAndLookEmpty(item);
            SetItem(item);
            _fromCellUsed = false;
        }

        public void ShowAsPrevious()
        {
            IsHidden = false;
            IsActive = true;
            _fromCell.SetDarkened(true);
            _block.SetActive(true);
        }
        
        private void SetItem(MergeItem item)
        {
            _block.SetActive(true);
            IsActive = true;
            _icon.sprite = GC.ItemViews.GetIcon(item.item_id);
            IsHidden = false;
        }

        public void SetBack()
        {
            if(_fromCellUsed == false)
                _fromCell.ShowItemView();
            _fromCell.SetDarkened(false);
            _fromCell.PlayItemSet();
            Hide();
        }

        public void Hide()
        {
            IsActive = false;
            _block.SetActive(false);
            _fromCell?.SetDarkened(false);
            IsHidden = false;
        }

        public void HideView()
        {
            IsActive = false;
            _block.SetActive(false);
            IsHidden = true;
        }
        
        public void SetPosition(Vector3 position)
        {
            _movable.position = position;
        }
    }
}