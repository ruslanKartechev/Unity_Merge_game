using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeInput
    {
        void SetStash(IMergeStash stash);
        void Activate();
        void Deactivate();
        void TakeFromStash(MergeItem item);
    }
}