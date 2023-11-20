using UnityEngine;

namespace Game.Merging.View
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemDescription), fileName = nameof(MergeItemDescription), order = 11)]
    public class MergeItemDescription : ScriptableObject, IMergeItemDescription
    {
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;

        public string ItemName => _itemName;
        public string Description => _description;
    }
}