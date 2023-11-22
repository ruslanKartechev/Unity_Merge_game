using System.Collections.Generic;
using Game.Core;
using Game.Merging;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassUISuper : MergeClassUI
    {
        [Space(10)] [SerializeField] private MergeClassUIButton _classButton;
        [SerializeField] private float _eggCellSize;
        [SerializeField] private float _cardCellSize;
        [SerializeField] private GridLayoutGroup _gridLayout;
        private Dictionary<string, TimerEggMergeItem> _spawned = new Dictionary<string, TimerEggMergeItem>();

        public override int ItemsCount
        {
            get
            {
                var count = base.ItemsCount;
                foreach (var egg in GC.ItemsStash.SuperEggs)
                {
                    if (egg.IsTicking
                        && !SuperEggHelper.AlreadyAdded(egg.Item.item_id))
                        count++;
                }
                return count;
            }
        }

        
        public override void Init()
        {
            SetStashEggs();
        }

        private void SetSizeAndHighlights(bool yes)
        {
            if (yes)
            {
                _classButton.Highlight(true);
                // _gridLayout.cellSize = new Vector2(_eggCellSize,_eggCellSize);
            }
            else
            {
                _classButton.Highlight(false);
                // _gridLayout.cellSize = new Vector2(_cardCellSize,_cardCellSize);
            }   
        }
        
        public override void Show()
        {
            base.Show();
            SetStashEggs();
        }

        private void SetStashEggs()
        {
            var superEggs = GC.ItemsStash.SuperEggs;
            ClearSuperEggs();
            var specialCount = 0;
            foreach (var egg in superEggs)
            {
                Debug.Log($"Egg: {egg.name}, is ticking: {egg.IsTicking}");
                if(egg.IsTicking == false ||
                   _spawned.ContainsKey(egg.Item.item_id))
                    continue;
                if (SuperEggHelper.AlreadyAdded(egg.Item.item_id))
                    continue;
                specialCount++;
                var time = egg.TimeLeft;
                time.CorrectToZero();
                // var freeCell = GetFreeCell();
                // if(freeCell == null)                
                    // Debug.LogError($"[Superclass] Cannot get free cell for ticking super item");
                
                var view = Spawn(_spawnParent);
                view.OnUnlocked += OnUnlocked;
                view.Show(egg);
                _spawned.Add(egg.Item.item_id, view);  
            }
            SetSizeAndHighlights(specialCount > 0);
        }

        private MergeUICell GetFreeCell()
        {
            foreach (var item in _items)
            {
                if (item.Item == null)
                    return item;
            }
            return null;
        }
        
        private void OnUnlocked()
        {
            Show();   
            Init();
        }

        protected TimerEggMergeItem Spawn(Transform parent)
        {
            var instance = Instantiate(GC.ItemViews.GetSuperEggItemView(), parent);
            var rect = instance.GetComponent<RectTransform>();
            // rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one * .5f;
            return instance.GetComponent<TimerEggMergeItem>();
        }

        protected void ClearSuperEggs()
        {
            foreach (var item in _spawned)
                Destroy(item.Value.gameObject);
            _spawned.Clear();
        }
        
        
        
    }
}