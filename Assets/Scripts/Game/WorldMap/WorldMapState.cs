using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapState : WorldMapPart
    {
        [SerializeField] private WorldMapEnemyPacksRepository _enemyPacksRepository;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private Material _enemyGlowMaterial;
        [SerializeField] private Material _playerMaterial;
        [SerializeField] private Material _playerGlowMaterial;
        [Space(10)] 
        [SerializeField] private Transform _levelSpawnPoint;
        [Space(10)]
        [SerializeField] private Transform _vegitationSpawnPoint;
        [SerializeField] private WorldMapPlayerProps _playerProps;
        [SerializeField] private WorldMapEnemyProps _enemyProps;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _fog;
        [SerializeField] private bool _log;
        private bool _enemiesSpawned;
        private bool _isEnemy;
        
        public bool IsEnemy
        {
            get => _isEnemy;
            set => _isEnemy = value;
        }
        
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
        
        public Material EnemyGlowMaterial
        {
            get => _enemyGlowMaterial;
            set => _enemyGlowMaterial = value;
        }
        
        public Material PlayerGlowMaterial
        {
            get => _playerGlowMaterial;
            set => _playerGlowMaterial = value;
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
            // HideEnemyProps();
            _enemiesSpawned = true;
        }
        
        public override void HideLevel()
        {
            if(_levelInstance != null)
                _levelInstance.SetActive(false);   
        }

        public override void FogSetActive(bool active)
        {
            if (_fog == null)
                return;
            _fog.gameObject.SetActive(active);
        }

        public override void GlowSetActive(bool active)
        {
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                _isEnemy = _renderer.sharedMaterial != _playerMaterial;
            }
            #endif
            if (_isEnemy)
            {
                _renderer.sharedMaterial = active ? _enemyGlowMaterial : _enemyMaterial;
            }
            else
            {
                _renderer.sharedMaterial = active ? _playerMaterial : _playerGlowMaterial;
            }
        }

        public override void SetEnemyTerritory()
        {
            _renderer.sharedMaterial = _enemyMaterial;
            HidePlayerProps();
            ShowEnemyProps();
            _isEnemy = true;
        }

        public override void SetPlayerTerritory()
        {
            HideEnemyProps();
            FogSetActive(false);
            _renderer.sharedMaterial = _playerMaterial;
            if (_playerProps != null)
                _playerProps.gameObject.SetActive(true);
            _isEnemy = false;
        }

        public void ShowEnemyProps()
        {
            if(_log)
                Debug.Log("Show enemy props");
                    
            if (_enemyProps != null)
                _enemyProps.gameObject.SetActive(true);
            _isEnemy = true;
        }
        
        public void HideEnemyProps()
        {
            if(_log)
                Debug.Log("Hide enemy props");
            if (_enemyProps != null)
                _enemyProps.gameObject.SetActive(false);
        }

        public void ShowPlayerProps()
        {
            if(_playerProps != null)
                _playerProps.gameObject.SetActive(true);
            _isEnemy = false;
        }

        public void HidePlayerProps()
        {
            if(_playerProps != null)
                _playerProps.gameObject.SetActive(false);
        }

        public void SwitchCollider(bool on) => _collider.enabled = on;
        
        
        
        


        #region Editor
        private const string VegitationGOName = "Veg";
#if UNITY_EDITOR
        
        public GameObject WorldMapEnemyTerritoryProps => _enemyProps != null ? _enemyProps.gameObject : null;
        
        private void OnValidate()
        {
            // if (_levelSpawnPoint != null)
            // {
            //     var pos = _levelSpawnPoint.localPosition;
            //     pos.y = 0.059f;
            //     _levelSpawnPoint.localPosition = pos;
            // }
            // if(_collider == null)
            //     _collider = GetComponent<Collider>();
            //
            // if (FogPlane == null)
            // {
            //     for (var i = 0; i < transform.childCount; i++)
            //     {
            //         if (transform.GetChild(i).name.Contains("FogPlane"))
            //         {
            //             FogPlane = transform.GetChild(i).gameObject;
            //             break;
            //         }
            //     }
            // }
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