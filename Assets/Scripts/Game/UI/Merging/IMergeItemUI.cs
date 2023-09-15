using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeItemUI
    {
        MergeItem Item { get; set; }
        void SetEmpty();
        void ShowItemView();
        void SetMerged(MergeItem item);
        void SetDarkened(bool darkened);
        void PlayItemSet();
        void SetItemAndLookEmpty(MergeItem item);
    }
}