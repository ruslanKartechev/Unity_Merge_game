using System.Collections.Generic;
using Common.Utils;
using Game.Core;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    public class MergeManager : MonoBehaviour, IMergeManager
    {
        [SerializeField] private GroupGridBuilder gridBuilder;
        private IMergeInput _mergeInput;
        
        public IMergeInput MergeInput => _mergeInput;

        
        private void GetComponents()
        {
            _mergeInput = GetComponent<IMergeInput>();
        }
        
        public void Init()
        {
            GetComponents();
            gridBuilder.Spawn(GC.ActiveGroupSO.Group());
        }

        public void MoveToPlayLevel()
        {
            CLog.LogWHeader(nameof(MergeManager), "Play Button", "b", "w");
            var count = GetActiveGroupCount();
            if (count == 0)
            {
                CLog.LogWHeader(nameof(MergeManager), "ZERO Hunters in active group", "r");
                return;
            }
            GC.Input.Disable();
            GC.LevelManager.LoadCurrent();
        }

        private int GetActiveGroupCount()
        {
            var count = 0;
            var setup = GC.ActiveGroupSO.Group();
            for (var i = 0; i < setup.RowsCount; i++)
            {
                var row = setup.GetRow(i);
                for (var x = 0; x < row.CellsCount; x++)
                {
                    var cell = row.GetCell(x);
                    // Debug.Log($"Row: {i}, Cell: {x}, Null {cell.Item == null}");
                    if (cell.Item != null)
                        count++;
                }   
            }
            return count;
        }
        
        public void MergeAllInStash()
        {
            CLog.LogWHeader(nameof(MergeManager), "Merging all inside stash", "b", "w");
            var stash = GC.ItemsStash.Stash;
            int iterations = 0;
            foreach (var itemClass in stash.classes)
            {
                while (MergeClass(itemClass.items) && iterations < 50)
                {
                    iterations++;
                }
            }
            if(iterations >= 50)
                CLog.LogRed("Merging All. Iterations >= 50. Possible Error.");
        }

        private bool MergeClass(List<MergeItem> items)
        {
            for(var i = 0; i < items.Count - 1; i++)
            {
                for (var k = i + 1; k < items.Count; k++)
                {
                    // Debug.Log($"Trying, {items[i].item_id} and {items[k].item_id}");   
                    if (MergeTwoItems(items, items[i], items[k]))
                    {
                        // Debug.Log($"Yes, merge happened");
                        return true;
                    }   
                }
            }
            return false;
        }

        private bool MergeTwoItems(ICollection<MergeItem> items, MergeItem item1, MergeItem item2)
        {
            var mergedItem = GC.MergeTable.GetMergedItem(item1, item2);
            if (mergedItem == null)
                return false;
            items.Remove(item1);
            items.Remove(item2);
            items.Add(mergedItem);
            return true;
        }
    }
}