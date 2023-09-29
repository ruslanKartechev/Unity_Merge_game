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
        ClassBackgroundIcon GetIconBackground(string class_id);
    }
}