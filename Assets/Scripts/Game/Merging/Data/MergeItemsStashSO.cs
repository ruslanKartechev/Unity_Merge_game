using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemsStashSO), fileName = nameof(MergeItemsStashSO), order = 10)]
    public class MergeItemsStashSO : ScriptableObject
    {
        [SerializeField] private InitialStash _initialStash;
        [NonSerialized] private MergeItemsStash _currentStash;
        
        
        public MergeItemsStash Stash
        {
            get
            {
                if (_currentStash == null)
                {
                    CLog.LogBlue($"_currentStash == null, creating new from _initialStash");
                    _currentStash = MakeInitialStash();
                    _currentStash.Init();
                }
                return _currentStash;
            }
            set
            {
                _currentStash = value;
                _currentStash.Init();
            }
        }
        

        private MergeItemsStash MakeInitialStash()
        {
            var stash = new MergeItemsStash();
            stash.classes = new List<MergeItemsClass>(_initialStash.itemClasses.Count);
            foreach (var fromClass in _initialStash.itemClasses)
            {
                var itemClass = new MergeItemsClass
                {
                    class_id = fromClass.class_id,
                    items = new List<MergeItem>(fromClass.items.Count)
                };
                foreach (var itemSO in fromClass.items)
                {
                    var item = new MergeItem(itemSO.Item)
                    {
                        class_id = itemClass.class_id
                    };
                    itemClass.items.Add(item);
                }
                stash.classes.Add(itemClass);
                // Debug.Log($"+++ Added class id: {itemClass.class_id}");
            }
            return stash;
        }
        
        
        [System.Serializable]
        public class InitialStash
        {
            public List<StashClass> itemClasses = new List<StashClass>();
        }
        
        [System.Serializable]
        public class StashClass
        {
            public string class_id;
            public List<MergeItemSO> items = new List<MergeItemSO>();
        }
    }
}