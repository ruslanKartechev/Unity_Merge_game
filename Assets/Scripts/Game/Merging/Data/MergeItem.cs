using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class MergeItem
    {
        public string item_id;
        public int level;
        [HideInInspector] public string class_id;
        [HideInInspector] public float timerLeft;
        public bool unlocked;
        
        public MergeItem(){}

        public MergeItem(MergeItem other)
        {
            item_id = other.item_id;
            level = other.level;
            timerLeft = other.timerLeft;
            unlocked = other.unlocked;
            class_id = other.class_id;
        }
        
    }
}