using Game.UI.Merging;

namespace Game.Merging
{
    public partial class MergeInput
    {
        private class DraggedItem
        {
            public IGroupCellView fromCell;
            public IMergeItemView itemView;
            private bool _isFree;
            public bool IsFree => _isFree;

            public DraggedItem()
            {
                _isFree = true;
            }
            
            public void Init(IGroupCellView fromCell, IMergeItemView itemView)
            {
                this.itemView = itemView;
                this.fromCell = fromCell;
                _isFree = false;
            }

            public bool PutBack()
            {
                if (fromCell == null || !fromCell.IsFree)
                    return false;
                fromCell.PutItem(itemView);
                _isFree = true;
                return true;
            }

            public void SetFree()
            {
                _isFree = true;
            }

            public void ClearCellToo()
            {
                fromCell.RemoveItem();
                itemView.Destroy();
                SetFree();
            }
            
        }
    }
}