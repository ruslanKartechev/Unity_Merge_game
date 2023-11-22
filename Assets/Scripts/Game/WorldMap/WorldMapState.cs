using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapState : WorldMapPart
    {
        [SerializeField] private WorldMapEnemyPacksRepository _enemyPacksRepository;
        [SerializeField] private Transform _levelSpawnPoint;
        [Space(10)] 
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _fadeMaterial;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private Material _enemyGlowMaterial;
        [SerializeField] private Material _playerMaterial;
        [SerializeField] private Material _playerGlowMaterial;
        [Space(10)]
        [SerializeField] private Transform _vegitationSpawnPoint;
        [SerializeField] private WorldMapPlayerProps _playerProps;
        [SerializeField] private WorldMapEnemyProps _enemyProps;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _fog;
        [SerializeField] private GameObject _arrow;
        [Header("Optional")]
        [SerializeField] private MapBoss _boss;
        [SerializeField] private MapBonus _bonus;
        private WorldMapEnemyPack _spawnedEnemies;
        
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

        public override void SpawnLevelEnemies(SpawnLevelArgs args)
        {
            if (_boss != null)
            {
                FogSetActive(false);
                _boss.Show();
                return;
            }
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
                return;
            #endif
            // Debug.Log($"Spawning level enemies: {index}");
            FogSetActive(false);
            var levelPrefab = _enemyPacksRepository.GetPrefab(args.Index);
            var levelInstance = Instantiate(levelPrefab, transform).GetComponent<WorldMapEnemyPack>();
            _spawnedEnemies = levelInstance;
            if(args.Dead)
                levelInstance.ShowDead();
            else
                levelInstance.ShowActive();
            levelInstance.transform.localScale = Vector3.one * (1f / transform.parent.parent.localScale.x);
            levelInstance.transform.SetPositionAndRotation(_levelSpawnPoint.position, _levelSpawnPoint.rotation);
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
                _renderer.sharedMaterial = active ? _enemyGlowMaterial : _enemyMaterial;
            else
                _renderer.sharedMaterial = active ? _playerMaterial : _playerGlowMaterial;
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
            _boss?.Hide();
            HideEnemyProps();
            FogSetActive(false);
            _renderer.sharedMaterial = _playerMaterial;
            if (_playerProps != null)
                _playerProps.gameObject.SetActive(true);
            _isEnemy = false;
        }
        
        public override void ArrowSetActive(bool active)
        {
            _arrow?.SetActive(active);
        }

        public override void CollectBonus()
        {
            if (_bonus == null)
                return;
            _bonus.Collect();
        }

        public override void AnimateToPlayer(AnimateArgs args)
        {
            _boss?.OnConquered();
            StartCoroutine(AnimatingToPlayer(args.OnComplete, args.OnEnemyHidden, 
                args.ScaleDuration, args.FadeDuration));
        }

        public void ShowEnemyProps()
        {
            if (_enemyProps != null)
                _enemyProps.gameObject.SetActive(true);
            _isEnemy = true;
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
            _isEnemy = false;
        }

        public void HidePlayerProps()
        {
            if(_playerProps != null)
                _playerProps.gameObject.SetActive(false);
        }

        public void SwitchCollider(bool on) => _collider.enabled = on;

        private IEnumerator AnimatingToPlayer(Action onComplete, Action onMiddle, float duration, float fadeDuration)
        {
            var t1 = duration / 2f;
            yield return null;
            if (_spawnedEnemies != null)
            {
                var levelProps = _spawnedEnemies.GetComponent<WorldMapEnemyProps>();
                StartCoroutine(levelProps.AnimatingDown(t1));           
            }
            yield return _enemyProps.AnimatingDown(t1);
            _enemyProps.Hide();
            yield return null;
            onMiddle.Invoke();
            StartCoroutine(Fading(_fadeMaterial, 0, 1, fadeDuration));

            _playerProps.gameObject.SetActive(true);
            yield return _playerProps.AnimateUp(t1);
            
            onComplete.Invoke();
        }

        private IEnumerator Fading(Material material, float from, float to, float duration)
        {
            _renderer.sharedMaterial = material;
            material.SetColor("_Color2", _enemyMaterial.GetColor("_Color"));
            material.SetTexture("_MainTex2", _enemyMaterial.GetTexture("_MainTex"));
            material.SetColor("_EmissionColor2", _enemyMaterial.GetColor("_EmissionColor"));
            material.SetFloat("_FadeThreshold", from);
            var elapsed = 0f;
            while (elapsed <= duration)
            {
                var val = Mathf.Lerp(from, to, elapsed / duration);
                material.SetFloat("_FadeThreshold", val);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // _renderer.sharedMaterial = _playerMaterial;
        }
        
        
        
        
        #region Editor
#if UNITY_EDITOR
        public GameObject WorldMapEnemyTerritoryProps => _enemyProps != null ? _enemyProps.gameObject : null;
        private const string PlayerSpawnName = "Player Spawn";
        private const string ArrowName = "Map Arrow";
        
        private void OnValidate()
        {
            // GetNewArrow();
        }

        private void GetNewArrow()
        {
            if (_arrow == null)
            {
                _arrow = transform.Find(ArrowName).gameObject;
                UnityEditor.EditorUtility.SetDirty(this);
            }   
        }
        
        private void TryGetPlayerSpawn()
        {
            var spawnPoint = transform.Find(PlayerSpawnName);
            if (spawnPoint == null)
            {
                var spawn = new GameObject(PlayerSpawnName);
                spawn.transform.SetParent(transform);
                spawn.transform.localScale = Vector3.one;
                spawn.transform.SetPositionAndRotation(transform.position, transform.rotation);
                _playerSpawnPoint = spawn.transform;
                UnityEditor.EditorUtility.SetDirty(this);
            }
            else
            {
                _playerSpawnPoint = spawnPoint;
                UnityEditor.EditorUtility.SetDirty(this);
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