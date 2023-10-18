using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeClassUISuper : MergeClassUI
    {
        [Space(10)] [SerializeField] private MergeClassUIButton _classButton;
        private Dictionary<string, TimerEggMergeItem> _spawned = new Dictionary<string, TimerEggMergeItem>();

        
        
        public override void Init()
        {
            var yes = false;
            foreach (var egg in GC.ItemsStash.SuperEggs)
            {
                if (egg.IsTicking)
                    yes = true;
            }
            if (yes)
                _classButton.Highlight(true);
            else
                _classButton.Highlight(false);
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