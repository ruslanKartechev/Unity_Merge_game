using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Hunting
{
    public class PackEnemyPoint : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField] private string _enemyName = "Enemy 1";
        [SerializeField] private bool _changeName = true;
        [SerializeField] private bool _autoMoveSpawned = true;
        [Space(10)]
        [SerializeField] private PreyPack _pack;
        [SerializeField] private Transform _moveToPoint;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _parent;
        [SerializeField] private List<GameObject> _prefabs;
        [Space(10)]
        [SerializeField] private GameObject _currentPrefab;
        [Space(10)]
        [SerializeField] private GameObject _instance;
        [SerializeField] private int _index;
        
        [Space(10)] 
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private Color _color;
        [SerializeField] private float _rad;

        public void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _rad);
            if (_moveToPoint != null)
            {
                Gizmos.DrawSphere(_moveToPoint.position, _rad);
                Gizmos.DrawLine(transform.position, _moveToPoint.position);
            }
            Gizmos.color = oldColor;
        }

        public void FetchRefs()
        {
            if (_pack == null)
            {
                _pack = transform.parent.parent.GetComponentInChildren<PreyPack>();
            }
            if (_spawnPoint == null)
                _spawnPoint = transform;
            if (_parent == null)
                _parent = transform.parent;
        }

        public void Move()
        {
            if (_autoMoveSpawned && _instance != null)
            {
                _instance.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            }
        }
        
        public void Spawn()
        {
            Clear();
            if (_prefabs.Count == 0)
                return;
            var prefab = _currentPrefab;
            var spawnPoint = _spawnPoint;
            var instance = PrefabUtility.InstantiatePrefab(prefab, _parent) as GameObject;
            instance.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            _instance = instance;
            var enemy = _instance.GetComponent<IPrey>();
            _pack.Prey.Add((MonoBehaviour)enemy);
            var mover = _instance.GetComponent<PackUnitLocalMover>();
            mover.SetPoint(_moveToPoint);
            if (_changeName)
                instance.name = _enemyName;
        }

        public void Clear()
        {
            if(_instance != null)
                DestroyImmediate(_instance);
            for (var i = _pack.Prey.Count - 1; i >= 0; i--)
            {
                if(_pack.Prey[i] == null)
                    _pack.Prey.RemoveAt(i);
            }   
            UnityEditor.EditorUtility.SetDirty(_pack.gameObject);
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }

        public void SpawnNext()
        {
            _index++;
            CorrectIndex();
            _currentPrefab = _prefabs[_index];
        }

        public void SpawnPrev()
        {
            _index--;
            CorrectIndex();
            _currentPrefab = _prefabs[_index];
        }
        
        private void CorrectIndex()
        {
            _index = Mathf.Clamp(_index, 0, _prefabs.Count - 1);
        }
#endif
    }
}