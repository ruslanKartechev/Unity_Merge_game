using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemSpawner : MonoBehaviour, IMergeItemSpawner
    {
        public IMergeItem SpawnItem(IMergeCell cell, int level)
        {
            var prefab = Container.HuntersRepository.GetItemByLevel(level).GetPrefab();
            var item = Instantiate(prefab, transform).GetComponent<IMergeItem>();
            cell.SpawnItem(item);
            return item;
        }

        public int MaxLevel => Container.HuntersRepository.GetMaxLevel();
    }
}