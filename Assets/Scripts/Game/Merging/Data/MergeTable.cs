using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeTable), fileName = nameof(MergeTable), order = 10)]
    public class MergeTable : ScriptableObject, IMergeTable
    {
        public MergeClassSO land;
        public MergeClassSO air;
        public MergeClassSO water;
        public MergeClassSO super;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            land.SetClassId();
            water.SetClassId();
            air.SetClassId();
            super.SetClassId();
        }
        #endif
        

        [System.Serializable]
        public class MergeClassSO
        {
            public string class_id;
            public List<MergeItemSO> items;

            public void SetClassId()
            {
                foreach (var item in items)
                    item.Item.class_id = class_id;
            }
        }
        
        public MergeItem GetItem(int level, string classId)
        {
            // Debug.Log($"Get item: {classId}, level: {level}");
            if (classId == land.class_id)
                return Get(land);
            if (classId == air.class_id)
                return Get(air);
            if (classId == water.class_id)
                return Get(water);
            if (classId == super.class_id)
                return Get(super);

            return null;
            
            MergeItem Get(MergeClassSO itemClass)
            {
                if (level >= itemClass.items.Count)
                    return null;
                var item = new MergeItem(itemClass.items[level].Item)
                {
                    class_id = classId
                };
                return item;
            }
        }

        public MergeItem GetMergedItem(MergeItem item1, MergeItem item2)
        {
            // Debug.Log($"Get merge item1: {item1.class_id}, item2: {item2.class_id}");
            if (item1.class_id != item2.class_id)
                return null;
            if (item1.level != item2.level)
                return null;
            return GetItem(item1.level + 1, item1.class_id);
        }
    }
}