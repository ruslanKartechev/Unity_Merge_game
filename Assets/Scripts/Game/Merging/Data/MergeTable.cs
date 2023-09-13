using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeTable), fileName = nameof(MergeTable), order = 10)]
    public class MergeTable : ScriptableObject, IMergeTable
    {
        public MergeItemsClass land;
        public MergeItemsClass air;
        public MergeItemsClass water;
        public MergeItemsClass super;
        
        
        public MergeItem GetItem(int level, string classId)
        {
            if (classId == land.class_id)
                return GetData(land);
            if (classId == air.class_id)
                return GetData(land);
            if (classId == water.class_id)
                return GetData(land);
            if (classId == super.class_id)
                return GetData(land);

            return null;
            MergeItem GetData(MergeItemsClass itemClass)
            {
                if (level >= itemClass.items.Count)
                    return null;
                return itemClass.items[level];
            }
        }

        public MergeItem GetItem(MergeItem item1, MergeItem item2)
        {
            if (item1.class_id != item2.class_id)
                return null;
            if (item1.level != item2.level)
                return null;
            return GetItem(item1.level + 1, item1.class_id);
        }
    }
}