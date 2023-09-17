using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemViews), fileName = nameof(MergeItemViews), order = 11)]
    public class MergeItemViews : ScriptableObject, IMergeItemViews
    {
        [SerializeField] private List<Data> _land_items;
        [SerializeField] private List<Data> _water_items;
        [SerializeField] private List<Data> _air_items;
        [SerializeField] private List<Data> _super_items;
        
        [NonSerialized] private Dictionary<string, Data> _table = new Dictionary<string, Data>();

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            var count = 0;
            count += _land_items.Count;
            count += _air_items.Count;
            count += _water_items.Count;
            count += _super_items.Count;
            
            _table = new Dictionary<string, Data>(count);
            foreach (var data in _land_items)
                _table.Add(data.ID, data);
            foreach (var data in _air_items)
                _table.Add(data.ID, data);
            foreach (var data in _water_items)
                _table.Add(data.ID, data);
            foreach (var data in _super_items)
                _table.Add(data.ID, data);
        }
        
        public GameObject GetPrefab(string id)
        {
            return _table[id].viewPrefab;
        }

        public Sprite GetIcon(string id)
        {
            return _table[id].uiIcon;
        }
        
        public IMergeItemDescription GetDescription(string id)
        {
            return _table[id].itemDescription;
        }
        
        [System.Serializable]
        public class Data
        {
            public MergeItemSO itemSO;
            public GameObject viewPrefab;
            public Sprite uiIcon;
            public MergeItemDescription itemDescription;

            public string ID => itemSO.Item.item_id;
            
        }

        [System.Serializable]
        public class MergeClassData
        {
            public string class_id;
            public List<Data> items;
        }
    }
}