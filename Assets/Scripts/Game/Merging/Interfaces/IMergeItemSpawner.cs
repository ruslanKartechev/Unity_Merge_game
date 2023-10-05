using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeItemSpawner
    {
        IMergeItemView SpawnItem(IGroupCellView cell, MergeItem item);
        IMergeItemView SpawnItem(MergeItem item);
        
    }
}