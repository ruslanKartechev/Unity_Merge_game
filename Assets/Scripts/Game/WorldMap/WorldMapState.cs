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
        [SerializeField] private GameObject _vegitation;
        [SerializeField] private WorldMapEnemyTerritoryProps _worldMapEnemyTerritoryProps;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;
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
            HideProps();
            _enemiesSpawned = true;
        }

        public override void ShowLevelNumber(int level)
        {
            _mapLevelNumber.SetLevel(level+1);
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
            ShowProps();
            if (_vegitation != null)
                _vegitation.gameObject.SetActive(false);
        }

        public override void SetPlayerTerritory()
        {
            HideProps();
            _renderer.sharedMaterial = _playerMaterial;
            if (_vegitation != null)
                _vegitation.gameObject.SetActive(true);
            _mapLevelNumber.SetPlayer();
        }

        public void ShowProps()
        {
            if (_enemiesSpawned)
                return;
            if (_worldMapEnemyTerritoryProps != null)
                _worldMapEnemyTerritoryProps.gameObject.SetActive(true);
        }
        
        public void HideProps()
        {
            if (_worldMapEnemyTerritoryProps != null)
                _worldMapEnemyTerritoryProps.gameObject.SetActive(false);
        }



        #region Editor
        private const string VegitationGOName = "Veg";
#if UNITY_EDITOR
        private void OnValidate()
        {
            // if (_renderer == null)
            //     GetRenderer();
            // if(_worldMapEnemyTerritoryProps == null)
            //     GetEnemyProps();
            if (_levelSpawnPoint != null)
            {
                var pos = _levelSpawnPoint.localPosition;
                pos.y = 0.059f;
                _levelSpawnPoint.localPosition = pos;
            }

            if (_vegitation == null)
            {
                var vegTr = transform.Find(VegitationGOName);
                if (vegTr != null)
                    _vegitation = vegTr.gameObject;           
            }
        }

        
        
        [ContextMenu("Get Enemy Props")]
        public void GetEnemyProps()
        {
            _worldMapEnemyTerritoryProps = GetComponentInChildren<WorldMapEnemyTerritoryProps>();
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