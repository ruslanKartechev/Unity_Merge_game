using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeItemSpawner
    {
        IMergeItem SpawnItem(IMergeCell cell, int level);
        int MaxLevel { get; }
    }
}