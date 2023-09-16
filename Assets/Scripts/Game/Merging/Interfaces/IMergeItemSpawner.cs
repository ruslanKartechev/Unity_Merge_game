using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeItemSpawner
    {
        IMergeItemView SpawnItem(IGroupCellView cell, MergeItem item);
        int MaxLevel { get; }
        IMergeItemView SpawnItem(MergeItem item);
        
    }
}