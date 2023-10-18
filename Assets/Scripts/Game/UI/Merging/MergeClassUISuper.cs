using System.Collections.Generic;
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
                    if (egg.IsTicking)
                        count++;
                }
                return count;
            }
        }

        
        public override void Init()
        {
            var yes = false;
            foreach (var egg in GC.ItemsStash.SuperEggs)
            {
                if (egg.IsTicking)
                    yes = true;
            }

            if (yes)
            {
                _classButton.Highlight(true);
                _gridLayout.cellSize = new Vector2(_eggCellSize,_eggCellSize);
            }
            else
            {
                _classButton.Highlight(false);
                _gridLayout.cellSize = new Vector2(_cardCellSize,_cardCellSize);
            }
        }

        
        
        public override void Show()
        {
            base.Show();
            var stash = GC.ItemsStash.SuperEggs;
            if (stash.Count == 0)
            {
                ClearSuperEggs();
                return;
            }
            
            foreach (var egg in stash)
            {
                Debug.Log($"Egg: {egg.name}, is ticking: {egg.IsTicking}");
                if(egg.IsTicking == false ||
                   _spawned.ContainsKey(egg.Item.item_id))
                    continue;
                var view = Spawn();
                view.Show(egg);
                _spawned.Add(egg.Item.item_id, view);
                view.OnUnlocked += OnUnlocked;
            }
         
        }

        
        private void OnUnlocked()
        {
            Show();   
            Init();
        }

        protected TimerEggMergeItem Spawn()
        {
            return Instantiate(GC.ItemViews.GetSuperEggItemView(), _spawnParent).GetComponent<TimerEggMergeItem>();
        }

        protected void ClearSuperEggs()
        {
            foreach (var item in _spawned)
                Destroy(item.Value.gameObject);
            _spawned.Clear();
        }
        
        
        
    }
}