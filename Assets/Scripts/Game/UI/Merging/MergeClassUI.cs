using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassUI : MonoBehaviour
    {
        [SerializeField] private List<MergeItemUI> _items;
        [SerializeField] private string _classId;
        
        public void Show()
        {
            var classData = GC.ItemsStash.Stash.GetClass(_classId);
            if (_items.Count < classData.items.Count)
            {
                Debug.Log("not enough UI items...");
                // need to spawn new ones
            }
            for (var i = 0; i < _items.Count; i++)
            {
                if (i < classData.items.Count)
                {
                    _items[i].Item = classData.items[i];
                    _items[i].ShowItemData();
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
    }
}