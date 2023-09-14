using System.Collections.Generic;

namespace Game.Merging
{
    [System.Serializable]
    public class MergeItemsClass
    {
        public string class_id;
        public List<MergeItem> items = new List<MergeItem>();
        
        public MergeItemsClass() {}
        
        public MergeItemsClass(MergeItemsClass other)
        {
            class_id = other.class_id;
            items = new List<MergeItem>(other.items.Count);
            foreach (var otherItem in other.items)
                items.Add(new MergeItem(otherItem));
        }

        public void SetClassToItems()
        {
            foreach (var item in items)
                item.class_id = class_id;
        }

        public void Sort()
        {
            items.Sort((item1, item2) =>
            {
                if (item1.level > item2.level)
                    return 1;
                if (item1.level < item2.level)
                    return -1;
                return 0;
            });
        }
    }
}