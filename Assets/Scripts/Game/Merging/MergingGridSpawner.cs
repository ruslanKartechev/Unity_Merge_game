using System.Collections.Generic;
using Game.UI.Merging;
using UnityEditor;
using UnityEngine;

namespace Game.Merging
{
    public class MergingGridSpawner : MonoBehaviour
    {
        [SerializeField] private bool _spawnOnXZ;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<GameObject> _spawned;

        [SerializeField] private float xSize = 1;
        [SerializeField] private float ySize = 2;
        [SerializeField] private float xSpace = 0.1f;
        [SerializeField] private float ySpace = 0.1f;
        [SerializeField] private float gapSpace;
        [SerializeField] private Transform _parent;
        private List<IList<IMergeCell>> _spawnedCells;
        private IMergeGridData _currentData;
        private IMergeItemSpawner _itemSpawner;
        private int _xCount;
        private int _yCount;
#if UNITY_EDITOR
        [Space(10)]
        public MergeGridRepository mergeGridRepository;
#endif

        public IList<IList<IMergeCell>> GetSpawnedCells() => _spawnedCells;

        public IList<IList<IMergeCell>> Spawn(IMergeGridData data, IMergeItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            _currentData = data;
            var rowsBeforeIndent = 1;
            _xCount = data.GetRow(0).CellsCount;
            _yCount = data.RowsCount;
            for (var i = 0; i < _yCount; i++)
            {
                if (!data.GetRow(i).IsAvailable)
                    break;
                rowsBeforeIndent = i;
            }
            SpawnGrid(rowsBeforeIndent);
            return _spawnedCells;
        }
        
        public void SpawnGrid(int rowsBeforeGap)
        {
            _spawnedCells = new List<IList<IMergeCell>>(_yCount);
            if (rowsBeforeGap <= 0)
                rowsBeforeGap = 1;
            Clear();
            var localY = 0f;
            if (_yCount % 2 == 0)
                localY -= (_yCount / 2 - 0.5f) * (ySize + ySpace) + gapSpace / 2;
            else
                localY -= (_yCount / 2) * (ySize + ySpace) + gapSpace / 2;
            
            for (var y = 0; y < _yCount; y++)
            {
                var yPos = localY + y * (ySize + ySpace);
                if (y > rowsBeforeGap)
                    yPos += gapSpace;
                SpawnRow(yPos, y);
            }
        }

        private void SpawnRow(float yPos, int y)
        {
            var localX = 0f;
            if (_xCount % 2 == 0)
                localX -= (_xCount / 2 - 0.5f) * (xSize + xSpace);
            else
                localX -= (_xCount / 2) * (xSize + xSpace);
            _spawnedCells.Add(new List<IMergeCell>(_xCount));
            for (var x = 0; x < _xCount; x++)
            {
                var xPos = localX;
                if(x > 0)
                    xPos = localX + (xSize + xSpace)* x;
                var localPosition = new Vector3(xPos, 0, yPos);
                if (!_spawnOnXZ)
                    localPosition = new Vector3(xPos, yPos, 0);
                var pos = _parent.TransformPoint(localPosition);
                SpawnAt(pos, x, y);                
            }
        }

        private void SpawnAt(Vector3 position, int x, int y)
        {
            GameObject instance = null;
            #if UNITY_EDITOR
            if (Application.isPlaying)
                instance = SpawnCell(position, x, y);
            else
            {
                instance = PrefabUtility.InstantiatePrefab(_prefab, _parent) as GameObject;
                instance.transform.position = position;
                // SetupCell(instance.gameObject.GetComponent<IMergeCell>(), x, y);
            }
            #else
                instance = SpawnCell(position, x, y);
            #endif
            _spawned.Add(instance);
        }

        private GameObject SpawnCell(Vector3 position, int x, int y)
        {
            var instance = Instantiate(_prefab, _parent);
            instance.transform.position = position;
            SetupCell(instance.gameObject.GetComponent<IMergeCell>(), x, y);
            return instance;
        }

        private void SetupCell(IMergeCell cell, int x, int y)
        {
            _spawnedCells[y].Add(cell);
            var data = _currentData.GetRow(y).GetCell(x);
            cell.X = x;
            cell.Y = y;
            if (_currentData.GetRow(y).IsAvailable)
            {
                cell.Init(data);
                if (data.SpawnItemLevel >= 0)
                    _itemSpawner.SpawnItem(cell, data.SpawnItemLevel);
            }
            else
                cell.SetInactive();   
        }
        
        public void Clear()
        {
            foreach (var go in _spawned)
            {
                DestroyImmediate(go);
            }
            _spawned.Clear();
        }
    }
}