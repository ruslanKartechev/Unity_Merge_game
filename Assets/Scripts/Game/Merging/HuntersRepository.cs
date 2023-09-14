using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HuntersRepository), fileName = nameof(HuntersRepository), order = 0)]
    public class HuntersRepository : ScriptableObject, IHuntersRepository
    {
        [SerializeField] private List<HunterData> _items;

        public IHunterData GetHunter(MergeItem item)
        {
            return null;
        }

        public int GetMaxLevel()
        {
            return _items.Count-1;
        }
    }
}