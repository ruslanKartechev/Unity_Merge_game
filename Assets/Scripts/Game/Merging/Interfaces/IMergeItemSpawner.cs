using Game.UI.Merging;

namespace Game.Merging.Interfaces
{
    public interface IMergeItemSpawner
    {
        IMergeItemView SpawnItem(IGroupCellView cell, MergeItem item);
        IMergeItemView SpawnItem(MergeItem item);
        
    }
}