using System.Collections;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItem : MonoBehaviour, IMergeItem
    {
        [SerializeField] private float _snapTime;
        [SerializeField] private int _itemLevel;
        [SerializeField] private Transform _movable;
        [SerializeField] private MaterialSwapper _materialSwapper;
        private Coroutine _snapping;
        
        public void OnSpawn()
        {
            var pos = _movable.position;
            _movable.position = pos + Vector3.up;
            StopSnapping();
            _snapping = StartCoroutine(Snapping(pos));
        }

        public int ItemLevel => _itemLevel;
        
        public void SetPositionRotation(Vector3 position, Quaternion rotation)
        {
            _movable.position = position;
            _movable.rotation = rotation;
        }

        public Vector3 GetPosition()
        {
            return _movable.position;
        }

        public void SetPosition(Vector3 position)
        {
            _movable.position = position;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void SnapToPos(Vector3 position)
        {
            StopSnapping();
            _snapping = StartCoroutine(Snapping(position));
        }

        public void OnPicked()
        {
            _materialSwapper.Switch();
        }

        public void OnReleased()
        {
            _materialSwapper.ReturnNormal();
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