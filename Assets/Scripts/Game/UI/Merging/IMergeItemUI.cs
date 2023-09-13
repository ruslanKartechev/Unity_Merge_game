using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeItemUI
    {
        MergeItem Item { get; set; }
        void SetEmpty();
        void ShowItemData();
        void PlayMerged();
        
    }
}