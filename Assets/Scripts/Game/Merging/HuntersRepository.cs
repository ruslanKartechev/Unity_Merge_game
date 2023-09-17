using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HuntersRepository), fileName = nameof(HuntersRepository), order = 0)]
    public class HuntersRepository : ScriptableObject, IHuntersRepository
    {
        [SerializeField] private List<Data> _items;
        private Dictionary<string, Data> _table;

        [System.Serializable]
        public class Data
        {
            public MergeItemSO item;
            public HunterData data;
        }

        public void Init()
        {
            _table = new Dictionary<string, Data>(_items.Count);
            foreach (var data in _items)
                _table.Add(data.item.Item.item_id, data);
        }
        
        
        public IHunterData GetHunterData(string id)
        {
            return _table[id].data;
        }

        public int GetMaxLevel()
        {
            return _items.Count - 1;
        }
    }
}