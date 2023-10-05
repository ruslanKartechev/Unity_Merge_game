using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemSpawner : MonoBehaviour, IMergeItemSpawner
    {
        public int MaxLevel => GC.HuntersRepository.GetMaxLevel();

        public IMergeItemView SpawnItem(IGroupCellView cell, MergeItem item)
        {
            var view = SpawnItem(item);
            cell.SpawnItem(view, item);
            return view;
        }

        public IMergeItemView SpawnItem(MergeItem item)
        {
            var prefab = GC.ItemViews.GetPrefab(item.item_id);
            var instance = Instantiate(prefab, transform);
            var view = instance.GetComponent<IMergeItemView>();
            view.SetSettings(GC.HuntersRepository.GetHunterData(item.item_id).GetSettings());
            return view;
        }
        
    }
}