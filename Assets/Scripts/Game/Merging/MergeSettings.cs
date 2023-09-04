using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeSettings), fileName = nameof(MergeSettings), order = 0)]
    public class MergeSettings : ScriptableObject
    {
        [SerializeField] private float _startCost;
        
        public float FirstLevelCost() => _startCost;

    }
}