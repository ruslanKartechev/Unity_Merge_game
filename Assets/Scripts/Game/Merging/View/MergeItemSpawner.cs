using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemSpawner : MonoBehaviour, IMergeItemSpawner
    {
        public IMergeItemView SpawnItem(IMergeCell cell, int level)
        {
            var prefab = GC.HuntersRepository.GetItemByLevel(level).GetPrefab();
            var item = Instantiate(prefab, transform).GetComponent<IMergeItemView>();
            cell.SpawnItem(item);
            return item;
        }

        public int MaxLevel => GC.HuntersRepository.GetMaxLevel();
    }
}