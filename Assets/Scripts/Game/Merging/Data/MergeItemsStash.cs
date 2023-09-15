﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class MergeItemsStash
    {
        public List<MergeItemsClass> classes = new List<MergeItemsClass>();

        public MergeItemsStash(){}
        public MergeItemsStash(MergeItemsStash other)
        {
            classes = new List<MergeItemsClass>(other.classes.Count);
            foreach (var @class in other.classes)
                classes.Add(new MergeItemsClass(@class));
        }

        public void Init()
        {
            foreach (var @class in classes)
                @class.SetClassToItems();
        }

        public void AddItem(MergeItem item)
        {
            var @class = GetClass(item.class_id);
            @class.items.Add(item);
        }
        
        public void RemoveItem(MergeItem item)
        {
            var @class = GetClass(item.class_id);
            @class.items.Remove(item);
        }
        
        public MergeItemsClass GetClass(string id)
        {
            foreach (var @class in classes)
            {
                if (id == @class.class_id)
                    return @class;
            }
            return null;
        }
    }
}