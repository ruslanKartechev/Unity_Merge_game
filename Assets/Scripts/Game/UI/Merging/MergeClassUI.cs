using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassUI : MonoBehaviour
    {
        [SerializeField] private List<MergeItemUI> _items;
        [SerializeField] private string _classId;

        public string ClassID => _classId;

        public int ItemsCount => GC.ItemsStash.Stash.GetClass(_classId).items.Count;
        
        public void Show()
        {
            var classData = GC.ItemsStash.Stash.GetClass(_classId);
            if (_items.Count < classData.items.Count)
            {
                Debug.Log("not enough UI items...");
                // need to spawn new ones
            }
            classData.Sort();
            var backgroundSprite = GC.ItemViews.GetIconBackground(_classId);
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].SetBackground(backgroundSprite);
                if (i < classData.items.Count)
                {
                    _items[i].Item = classData.items[i];
                    _items[i].ShowItemView();
                }
                else 
                    _items[i].SetEmpty();
                
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public MergeItemUI GetFirstFreeCell()
        {
            foreach (var itemUI in _items)
            {
                if (itemUI.Item == null)
                    return itemUI;
            }
            Debug.Log("NO FREE CELLS FOUND");
            return null;
        }
    }
}