using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemSpawner : MonoBehaviour, IMergeItemSpawner
    {
        public IMergeItemView SpawnItem(IGroupCellView cell, MergeItem item)
        {
            var prefab = GC.ItemViewRepository.GetPrefab(item.item_id);
            var instance = Instantiate(prefab, transform);
            var view = instance.GetComponent<IMergeItemView>();
            cell.SpawnItem(view, item);
            // Debug.Log($"Spawning item: {item.item_id}, gameobject: {instance.name}");
            return view;
        }

        public int MaxLevel => GC.HuntersRepository.GetMaxLevel();
    }
}