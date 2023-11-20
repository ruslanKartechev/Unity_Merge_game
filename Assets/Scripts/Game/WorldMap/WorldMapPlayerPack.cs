using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Core;
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
        
        public void JumpToAttack(float time)
        {
            // var delay = 0f;
            // var delayStep = .10f;
            // var dirs = new Vector2(6,5);
            foreach (var view in _spawned)
            {
                view.PrepareToJump();
                // view.JumpForward(dirs, delay, time, .6f);
                // delay += delayStep;
            }
            // StartCoroutine(Jumping(time));
        }
        
        public void JumpToCapture(Transform toPoint, float time)
        {
            var delay = 0f;
            var delayStep = .075f;
            var localPositions = new List<Vector3>(_spawned.Count);
            foreach (var view in _spawned)
            {
                localPositions.Add(view.transform.localPosition);
                view.transform.SetParent(transform.parent);
            }
            transform.SetPositionAndRotation(toPoint.position, toPoint.rotation);
            for (var i = 0; i < _spawned.Count; i++)
            {
                var view = _spawned[i];
                var endPoint = transform.TransformPoint(localPositions[i]);
                view.JumpToPoint(endPoint, delay, time, 1f);
                delay += delayStep;     
                view.transform.SetParent(transform);
            }
        }

        // private IEnumerator JumpingPack(Vector3 endPos, Quaternion endRot, float duration)
        // {
        //     var tr = transform;
        //     var scale = tr.localScale.x;
        //     var endScale = scale * _minScale;
        //     // yield return Scaling(scale, endScale, duration / 2f);
        //     transform.localScale = Vector3.one * endScale;
        //     tr.SetPositionAndRotation(endPos, endRot);
        //     yield return Scaling(endScale, scale, duration / 2f);
        // }

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
            var items = MergeHelper.GetAllItems(pack);
            
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