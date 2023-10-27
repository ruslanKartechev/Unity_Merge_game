using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapState : WorldMapPart
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private Material _playerMaterial;
        [Space(10)] 
        [SerializeField] private Transform _levelSpawnPoint;
        [SerializeField] private WorldMapLevelNumber _mapLevelNumber;
        [Space(10)]
        [SerializeField] private Transform _vegitationSpawnPoint;
        [SerializeField] private GameObject _vegitationPrefab;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;

        public WorldMapCameraPoint CameraPoint
        {
            get => _cameraPoint;
            set => _cameraPoint = value;
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void SpawnLevelEnemies(int index)
        {
            Debug.Log($"Spawning level enemies: {index}");
        }

        public override void ShowLevelNumber(int level)
        {
            _mapLevelNumber.SetLevel(level);
            _mapLevelNumber.Show();
        }

        public override void HideLevelNumber()
        {
            _mapLevelNumber.Hide();
        }

        public override void HideLevel()
        {
            if(_levelInstance != null)
                _levelInstance.SetActive(false);   
        }

        public override void SetEnemyTerritory()
        {
            _renderer.sharedMaterial = _enemyMaterial;
            _mapLevelNumber.SetEnemy();
        }

        public override void SetPlayerTerritory()
        {
            _renderer.sharedMaterial = _playerMaterial;
            if (_vegitationPrefab != null)
            {
                var vegitationInstance = Instantiate(_vegitationPrefab, _vegitationSpawnPoint.position, _vegitationSpawnPoint.rotation, transform);
            }
            _mapLevelNumber.SetPlayer();
        }



        #region Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_renderer == null)
                GetRenderer();
        }

        public void GetRenderer()
        {
            _renderer = GetComponentInChildren<Renderer>();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public Material EnemyMaterial
        {
            get => _enemyMaterial;
            set => _enemyMaterial = value;
        }
        
        public Material PlayerMaterial
        {
            get => _playerMaterial;
            set => _playerMaterial = value;
        }

        public Transform VegitationSpawnPoint
        {
            get => _vegitationSpawnPoint;
            set => _vegitationSpawnPoint = value;
        }

        public Transform LevelSpawnPoint
        {
            get => _levelSpawnPoint;
            set => _levelSpawnPoint = value;
        }


        public WorldMapLevelNumber MapLevelNumber
        {
            get => _mapLevelNumber;
            set => _mapLevelNumber = value;
        }


        [ContextMenu("Set Camera To This")] 
        public void SetCameraToThis()
        {
            if (_cameraPoint.Point == null)
            {
                Debug.Log("No Camera Point Transform");
                return;
            }
            var cam = FindObjectOfType<MapCamera>();
            if (cam == null)
            {
                Debug.Log("No MapCamera Found");
                return;
            }
            cam.SetPosition(_cameraPoint);
        }

        [ContextMenu("Calculate offset to camera")]
        public void CalculateOffsetToCamera()
        {
            if (_cameraPoint.Point == null)
            {
                Debug.Log("No Camera Point Transform");
                return;
            }
            var cam = FindObjectOfType<MapCamera>();
            if (cam == null)
            {
                Debug.Log("No MapCamera Found");
                return;
            }

            var offset = cam.transform.position - _cameraPoint.Point.position;
            _cameraPoint.Offset = offset;
        }
#endif
        #endregion

    }
}