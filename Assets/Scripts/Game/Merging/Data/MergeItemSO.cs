using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemSO), fileName = nameof(MergeItemSO), order = 10)]
    public class MergeItemSO : ScriptableObject
    {
        public MergeItem Item;
    }
}