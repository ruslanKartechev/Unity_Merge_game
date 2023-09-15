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
        [SerializeField] private TextMeshProUGUI _levelText;
        private IMergeItemUI _fromCell;
        public bool IsActive { get; set; }
        public IMergeItemUI FromCell => _fromCell;
        private bool _fromCellUsed;
        
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

        private void SetItem(MergeItem item)
        {
            _block.SetActive(true);
            IsActive = true;
            _icon.sprite = GC.ItemViewRepository.GetIcon(item.item_id);
            _levelText.text = $"{item.level + 1}";
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
            _block.SetActive(false);
            IsActive = false;
            _fromCell?.SetDarkened(false);
        }

        public void SetPosition(Vector3 position)
        {
            _movable.position = position;
        }
    }
}