using System;
using EditorUtils;
using TMPro;
using UnityEditor;
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
        [SerializeField] private GameObject _levelNumber;
        [SerializeField] private TextMeshPro _levelText;
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
            _levelNumber.SetActive(true);
            _levelText.text = $"{level}";
        }

        public override void HideLevelNumber()
        {
            _levelNumber.SetActive(false);
        }

        public override void HideLevel()
        {
            if(_levelInstance != null)
                _levelInstance.SetActive(false);   
        }

        public override void SetEnemyTerritory()
        {
            _renderer.sharedMaterial = _enemyMaterial;
        }

        public override void SetPlayerTerritory()
        {
            _renderer.sharedMaterial = _playerMaterial;
            var vegitationInstance = Instantiate(_vegitationPrefab, _vegitationSpawnPoint.position, _vegitationSpawnPoint.rotation, transform);
            
        }
        
        
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
    }

    
    
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(WorldMapState))]
    public class WorldMapStateEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            var me = target as WorldMapState;
            if (EU.ButtonBig("Set Camera", Color.white))
            {
                me.SetCameraToThis();
            }
            if (EU.ButtonBig("Get Offset", Color.white))
            {
                me.CalculateOffsetToCamera();
            }

        }
    }
    #endif
    
}