using UnityEngine;

namespace Game.Merging
{
    public interface IMergeItemViewRepository
    {
        GameObject GetPrefab(string id);
        Sprite GetIcon(string id);
        IMergeItemDescription GetDescription(string id);
    }
}