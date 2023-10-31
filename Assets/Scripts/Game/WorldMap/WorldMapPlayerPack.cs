using System.Collections.Generic;
using Game.Merging;
using UnityEngine;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(100)]
    public class WorldMapPlayerPack : MonoBehaviour
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private List<Transform> _points;
        [SerializeField] private List<MergeItemView> _spawned;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            var pack = GC.ActiveGroupSO.Group();
            var items = GroupHelper.GetAllItems(pack);
            
            if (items.Count == 1)
            {
                Debug.Log("only one case");
                var point = _points[1];
                var prefab = GC.ItemViews.GetPrefab(items[0].item_id);
                var instance = Instantiate(prefab, point.position, point.rotation, transform);
                var view = instance.GetComponent<MergeItemView>();
                view.DisplaySetActive(false);
                _spawned.Add(view);
                return;
            }
            var count = 0;
            foreach (var item in items)
            {
                if (MergeItem.Empty(item))
                    continue;
                var prefab = GC.ItemViews.GetPrefab(item.item_id);
                var point = _points[count];
                var instance = Instantiate(prefab, point.position, point.rotation, transform);
                var view = instance.GetComponent<MergeItemView>();
                view.DisplaySetActive(false);
                _spawned.Add(view);
                count++;
                if (count == _maxCount)
                    break;
            }
        }

    }
}