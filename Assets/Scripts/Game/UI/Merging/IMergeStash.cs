using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeStash
    {
        void TakeItem(MergeItem item);
        void TakeToStash(MergeItem item);
    }
}