using TMPro;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapState : WorldMapPart
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _enemyMaterial;
        [SerializeField] private Material _playerMaterial;
        [Space(10)] 
        [SerializeField] private GameObject _levelNumber;
        [SerializeField] private TextMeshPro _levelText;
        [Space(10)]
        [SerializeField] private Transform _vegitationSpawnPoint;
        [SerializeField] private GameObject _vegitationPrefab;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _cameraPoint;

        public WorldMapCameraPoint CameraPoint => _cameraPoint;
        
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
        
    }
}