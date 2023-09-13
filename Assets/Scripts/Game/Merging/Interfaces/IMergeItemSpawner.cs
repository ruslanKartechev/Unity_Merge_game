using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeItemSpawner
    {
        IMergeItemView SpawnItem(IMergeCell cell, int level);
        int MaxLevel { get; }
    }
}