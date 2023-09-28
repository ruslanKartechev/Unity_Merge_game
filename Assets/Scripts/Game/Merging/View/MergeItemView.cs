using System.Collections;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemView : MonoBehaviour, IMergeItemView
    {
        [SerializeField] private float _snapTime;
        [SerializeField] private Transform _movable;
        [SerializeField] private Vector3 _spawnedPositionOffset;
        private Coroutine _snapping;
        private IMergeItemHighlighter _highlighter;

        
        private void Awake()
        {
            _highlighter = GetComponent<IMergeItemHighlighter>();
        }

        public Quaternion Rotation
        {
            get => _movable.rotation;
            set => _movable.rotation = value;
        }
 
        private MergeItem _item;
        public MergeItem Item
        {
            get => _item;
            set => _item = value;
        }
        
        public void OnSpawn()
        {
            var pos = _movable.position;
            CorrectedPosition(ref pos);
            _movable.position = pos + Vector3.up;
            StopSnapping();
            _snapping = StartCoroutine(Snapping(pos));
        }

        public void SetPositionRotation(Vector3 position, Quaternion rotation)
        {
            _movable.position = position;
            _movable.rotation = rotation;
        }

        public void SetDraggedPosition(Vector3 position)
        {
            _movable.position = position + _spawnedPositionOffset;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void SnapToPos(Vector3 position)
        {
            CorrectedPosition(ref position);
            _movable.position = position + Vector3.up;
            StopSnapping();
            _snapping = StartCoroutine(Snapping(position));
        }

        public void OnPicked()
        {
            _highlighter.Highlight();
        }

        public void OnReleased()
        {
            _highlighter.Normal();
        }

        private void CorrectedPosition(ref Vector3 position)
        {
            position += _spawnedPositionOffset;
        }

        private void StopSnapping()
        {
            if(_snapping != null)
                StopCoroutine(_snapping);   
        }

        private IEnumerator Snapping(Vector3 endPos)
        {
            var elapsed = 0f;
            var startPos = _movable.position;
            while (elapsed < _snapTime)
            {
                _movable.position = Vector3.Lerp(startPos, endPos, elapsed / _snapTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _movable.position = endPos;
        }
    }
}