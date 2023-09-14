using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemSpawner : MonoBehaviour, IMergeItemSpawner
    {
        public IMergeItemView SpawnItem(IGroupCell cell, MergeItem item)
        {
            var prefab = GC.ItemViewRepository.GetPrefab(item.item_id);
            var spawned = Instantiate(prefab, transform).GetComponent<IMergeItemView>();
            cell.SpawnItem(spawned, item);
            return spawned;
        }

        public int MaxLevel => GC.HuntersRepository.GetMaxLevel();
    }
}