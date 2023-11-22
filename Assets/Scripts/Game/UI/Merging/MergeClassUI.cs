using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassUI : MonoBehaviour
    {
        [SerializeField] protected List<MergeUICell> _items;
        [SerializeField] protected Transform _spawnParent;
        [SerializeField] protected MergeAreaSliderController _mergeAreaSlider;
        [SerializeField] protected RectTransform _scrollViewRect;
        [SerializeField] private string _classId;

        protected const int SliderEnableCount = 8;
        
        public string ClassID => _classId;

        public virtual int ItemsCount => GC.ItemsStash.Stash.GetClass(_classId).items.Count;

        public virtual void Init()
        {
        }
        
        public MergeUICell GetItemUI(string id)
        {
            foreach (var item in _items)
            {
                if(item.Item == null)
                    continue;
                if (item.Item.item_id == id)
                    return item;
            }
            return null;
        }
        
        public virtual void Show()
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
            CheckScrollBar();
            gameObject.SetActive(true);
        }

        

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        protected void SpawnAdditional(int count)
        {
            var spawned = 0;
            var i = 0;
            var i_max = 500;
            var bcg = GC.ItemViews.GetIconBackground(_classId);
            var prefab = bcg.cellPrefab;
            while (spawned < count && i < i_max)
            {
                var instance = Instantiate(prefab, _spawnParent);
                var ui = instance.GetComponent<MergeUICell>();
                ui.SetEmpty();
                _items.Add(ui);
                spawned++;
                i++;
            }
            if (i >= i_max)
                Debug.LogError("ERROR When spawning new merge class UI icons !");
        }

        public MergeUICell GetFirstFreeCell()
        {
            foreach (var itemUI in _items)
            {
                if (itemUI.Item == null)
                    return itemUI;
            }
            Debug.Log("No free cell, spawning additional");
            SpawnAdditional(2);
            return GetFirstFreeCell();
        }
        
        public MergeUICell GetFirstCellWithItem()
        {
            foreach (var itemUI in _items)
            {
                if (itemUI.Item != null)
                    return itemUI;
            }
            Debug.Log("No Cells with items");
            return null;
        }

        protected void CheckScrollBar()
        {
            MergeAreaSizeAdjuster.AdjustSize(_scrollViewRect, _items.Count);
            if(_items.Count > SliderEnableCount)
                _mergeAreaSlider.Enable();
            else
                _mergeAreaSlider.Disable();   
        }
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_mergeAreaSlider == null)
            {
                _mergeAreaSlider = GetComponentInChildren<MergeAreaSliderController>();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}