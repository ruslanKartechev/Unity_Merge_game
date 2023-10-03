using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassUI : MonoBehaviour
    {
        [SerializeField] private List<MergeItemUI> _items;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] private string _classId;

        public string ClassID => _classId;

        public int ItemsCount => GC.ItemsStash.Stash.GetClass(_classId).items.Count;
        
        public void Show()
        {
            var classData = GC.ItemsStash.Stash.GetClass(_classId);
            if (_items.Count < classData.items.Count)
            {
                var spawnCount = (classData.items.Count - _items.Count);
                SpawnAdditional(spawnCount);
            }
            classData.Sort();
            for (var i = 0; i < _items.Count; i++)
            {
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

        private void SpawnAdditional(int count)
        {
            var spawned = 0;
            var i = 0;
            var i_max = 500;
            var bcg = GC.ItemViews.GetIconBackground(_classId);
            var prefab = bcg.cellPrefab;
            while (spawned < count && i < i_max)
            {
                var instance = Instantiate(prefab, _spawnParent);
                var ui = instance.GetComponent<MergeItemUI>();
                _items.Add(ui);
                spawned++;
                i++;
            }
            if (i >= i_max)
                Debug.LogError("ERROR When spawning new merge class UI icons !");
        }

        public MergeItemUI GetFirstFreeCell()
        {
            foreach (var itemUI in _items)
            {
                if (itemUI.Item == null)
                    return itemUI;
            }
            Debug.Log("No free cell, spawning additional");
            SpawnAdditional(4);
            return null;
        }
        
        public MergeItemUI GetFirstCellWithItem()
        {
            foreach (var itemUI in _items)
            {
                if (itemUI.Item != null)
                    return itemUI;
            }
            Debug.Log("No Cells with items");
            return null;
        }
    }
}