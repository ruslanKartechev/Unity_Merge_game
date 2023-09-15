using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeInput
    {
        void Init(IMergingPage page, IMergeItemSpawner spawner, IMergeInputUI mergeInputUI);
        void Activate();
        void Stop();
        void TakeItem(MergeItem item);
    }
}