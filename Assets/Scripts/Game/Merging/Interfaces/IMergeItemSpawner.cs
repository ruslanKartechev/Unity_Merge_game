using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeItemSpawner
    {
        IMergeItemView SpawnItem(IGroupCell cell, MergeItem item);
        int MaxLevel { get; }
    }
}