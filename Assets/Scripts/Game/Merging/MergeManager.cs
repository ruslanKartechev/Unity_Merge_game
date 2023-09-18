using System.Collections.Generic;
using Common;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class MergeManager : MonoBehaviour
    {
        [SerializeField] private CameraPoint _cameraPoint;
        [SerializeField] private GroupGridBuilder gridBuilder;
        [SerializeField] private ActiveGroupSO _mergeRepository;
        private IMergeInput _mergeInput;

        public IMergeInput MergeInput => _mergeInput;
        
        private void GetComponents()
        {
            _mergeInput = GetComponent<IMergeInput>();
        }
        
        public void Init()
        {
            GetComponents();
            gridBuilder.Spawn(_mergeRepository.GetSetup());
        }

        public void MoveToPlayLevel()
        {
            CLog.LogWHeader(nameof(MergeManager), "Play Button", "b", "w");
            GC.LevelManager.LoadCurrent();
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
            for(var i = 1; i < items.Count; i++)
            {
                if (TryMerge(items, items[i], items[i - 1]))
                    return true;
            }
            return false;
        }

        private bool TryMerge(List<MergeItem> items, MergeItem item1, MergeItem item2)
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