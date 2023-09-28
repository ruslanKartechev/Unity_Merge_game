using UnityEngine;

namespace Game.Merging
{
    public interface IMergeItemViews
    {
        GameObject GetPrefab(string id);
        Sprite GetIcon(string id);
        IMergeItemDescription GetDescription(string id);
        
        GameObject GetLevelIconPrefab();
        float LevelIconsSpacing();
        Sprite GetIconBackground(string class_id);
    }
}