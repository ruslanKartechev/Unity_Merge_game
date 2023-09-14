using Game.Merging;
using UnityEngine;

namespace Game.UI.Merging
{
    public interface IMergeItemUI
    {
        MergeItem Item { get; set; }
        void SetEmpty();
        void ShowItemData();
        void SetMerged(MergeItem item);
        void SetDarkened(bool darkened);
        void PlayItemSet();
    }
}