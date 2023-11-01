using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Merging;
using UnityEngine;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(100)]
    public class WorldMapPlayerPack : MonoBehaviour
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private float _minScale = .25f;
        [SerializeField] private List<Transform> _points;
        [SerializeField] private List<MergeItemView> _spawned;

        public void SetPosition(Transform point)
        {
            transform.SetPositionAndRotation(point.position, point.rotation);
        }

        public void BounceToPosition(Transform toPoint, float duration)
        {
            StartCoroutine(Bouncing(toPoint.position, toPoint.rotation, duration));
        }

        private IEnumerator Bouncing(Vector3 endPos, Quaternion endRot, float duration)
        {
            var tr = transform;
            var scale = tr.localScale.x;
            var endScale = scale * _minScale;
            // yield return Scaling(scale, endScale, duration / 2f);
            transform.localScale = Vector3.one * endScale;
            tr.SetPositionAndRotation(endPos, endRot);
            yield return Scaling(endScale, scale, duration / 2f);
        }

        private IEnumerator Scaling(float from, float to, float time)
        {
            var elapsed = 0f;
            while (elapsed <= time)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(from, to , elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one * to;
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

        public void Jump(float time)
        {
            foreach (var view in _spawned)
                view.PlayAttackAnim();
            StartCoroutine(Jumping(time));
        }

        private IEnumerator Jumping(float time)
        {
            var elapsed = 0f;
            var tr = transform;
            var pos = tr.position;
            var final = pos + tr.forward * 5f;
            var up = Vector3.Lerp(pos, final, .5f) + Vector3.up * 5f;
            while (elapsed <= time)
            {
                var t  = elapsed / time;
                tr.position = Bezier.GetPosition(pos, up, final, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}