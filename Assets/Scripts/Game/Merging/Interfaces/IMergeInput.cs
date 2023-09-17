using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeInput
    {
        void SetUI(IMergingPage page, IMergeInputUI mergeInputUI);
        void Activate();
        void Deactivate();
        void TakeItem(MergeItem item);
    }
}