﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class MergeItemUIBackground
    {
 
    }
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemViews), fileName = nameof(MergeItemViews), order = 11)]
    public class MergeItemViews : ScriptableObject, IMergeItemViews
    {
        [SerializeField] private List<Data> _land_items;
        [SerializeField] private List<Data> _water_items;
        [SerializeField] private List<Data> _air_items;
        [SerializeField] private List<Data> _super_items;
        [Space(10)] 
        [SerializeField] private GameObject _itemLevelIcon;
        [SerializeField] private float _levelIconsSpacing;
        [Space(10)] 
        [SerializeField] private List<ClassBackgroundIcon> _backgroundIcons;

        [NonSerialized] private Dictionary<string, Data> _table = new Dictionary<string, Data>();
        [NonSerialized] private Dictionary<string, ClassBackgroundIcon> _iconBackgroundsTable = new Dictionary<string, ClassBackgroundIcon>();


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
            InitClassBackgroundIcons();
        }

        private void InitClassBackgroundIcons()
        {
            _iconBackgroundsTable = new Dictionary<string, ClassBackgroundIcon>(_backgroundIcons.Count);
            foreach (var iconData in _backgroundIcons)
                _iconBackgroundsTable.Add(iconData.class_id, iconData);
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

        public GameObject GetLevelIconPrefab()
        {
            return _itemLevelIcon;
        }

        public float LevelIconsSpacing()
        {
            return _levelIconsSpacing;
        }

        public ClassBackgroundIcon GetIconBackground(string class_id)
        {
            return _iconBackgroundsTable[class_id];
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
    }
    
    [System.Serializable]
    public class ClassBackgroundIcon
    {
        public string class_id;
        public Sprite background;
        public Sprite fide;
    }
}