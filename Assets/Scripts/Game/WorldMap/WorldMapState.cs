using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapState : WorldMapPart
    {
        [SerializeField] private WorldMapEnemyPacksRepository _enemyPacksRepository;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private Material _playerMaterial;
        [Space(10)] 
        [SerializeField] private Transform _levelSpawnPoint;
        [SerializeField] private WorldMapLevelNumber _mapLevelNumber;
        [Space(10)]
        [SerializeField] private Transform _vegitationSpawnPoint;
        [SerializeField] private WorldMapPlayerProps _playerProps;
        [SerializeField] private WorldMapEnemyProps _enemyProps;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _fog;
        private bool _enemiesSpawned;
        
        public override WorldMapCameraPoint CameraPoint
        {
            get => _cameraPoint;
            set => _cameraPoint = value;
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
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
                return;
            #endif
            Debug.Log($"Spawning level enemies: {index}");
            var levelPrefab = _enemyPacksRepository.GetPrefab(index);
            var levelInstance = Instantiate(levelPrefab, transform);
            levelInstance.transform.localScale = Vector3.one * (1f / transform.parent.parent.localScale.x);
            levelInstance.transform.SetPositionAndRotation(_levelSpawnPoint.position, _levelSpawnPoint.rotation);
            HideEnemyProps();
            _enemiesSpawned = true;
        }

        public override void ShowLevelNumber(int level)
        {
            if (_mapLevelNumber == null) return;
            _mapLevelNumber.SetLevel(level+1);
            _mapLevelNumber.Show();
        }

        public override void HideLevelNumber()
        {
            if (_mapLevelNumber == null) return;
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
            ShowEnemyProps();
            HidePlayerProps();
            
        }

        public override void SetPlayerTerritory()
        {
            HideEnemyProps();
            _renderer.sharedMaterial = _playerMaterial;
            if (_playerProps != null)
                _playerProps.gameObject.SetActive(true);
        }

        public void ShowEnemyProps()
        {
            if (_enemyProps != null)
                _enemyProps.gameObject.SetActive(true);
        }
        
        public void HideEnemyProps()
        {
            if (_enemyProps != null)
                _enemyProps.gameObject.SetActive(false);
        }

        public void ShowPlayerProps()
        {
            if(_playerProps != null)
                _playerProps.gameObject.SetActive(true);
        }

        public void HidePlayerProps()
        {
            if(_playerProps != null)
                _playerProps.gameObject.SetActive(false);
        }

        public void SwitchCollider(bool on) => _collider.enabled = on;
        
        public void ShowFog()
        {
            if(_fog != null)
                _fog.gameObject.SetActive(true);
        }

        public void HideFog()
        {
            if(_fog != null)
                _fog.gameObject.SetActive(false);   
        }



        #region Editor
        private const string VegitationGOName = "Veg";
#if UNITY_EDITOR
        
        public GameObject WorldMapEnemyTerritoryProps => _enemyProps != null ? _enemyProps.gameObject : null;
        
        private void OnValidate()
        {
            if (_levelSpawnPoint != null)
            {
                var pos = _levelSpawnPoint.localPosition;
                pos.y = 0.059f;
                _levelSpawnPoint.localPosition = pos;
            }
            if(_collider == null)
                _collider = GetComponent<Collider>();
            
            if (FogPlane == null)
            {
                for (var i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Contains("FogPlane"))
                    {
                        FogPlane = transform.GetChild(i).gameObject;
                        break;
                    }
                }
            }
        }
        
        public GameObject FogPlane
        {
            get => _fog;
            set
            {
                _fog = value;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        
        public void SetEnemyProps(WorldMapEnemyProps props)
        {
            _enemyProps = props;
        }

        public void SetPlayerProps(WorldMapPlayerProps props)
        {
            _playerProps = props;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        
        [ContextMenu("Get Renderer")]
        public void GetRenderer()
        {
            _renderer = GetComponentInChildren<Renderer>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        public void EditorSetPlayer()
        {
            SetPlayerTerritory();
        }

        public void EditorSetEnemy()
        {
            SetEnemyTerritory();
        }

        
#endif
        #endregion

    }
}