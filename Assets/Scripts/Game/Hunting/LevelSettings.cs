using System.Collections.Generic;
using Game.Shop;
using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelSettings), fileName = nameof(LevelSettings), order = 0)]
    public class LevelSettings :  ScriptableObject, ILevelSettings
    {
        [SerializeField] private LevelEnvironment _environment;
        [SerializeField] private GameObject _preyPackPrefab;
        [SerializeField] private List<PreySettings> _preySettings;
        [SerializeField] private int _cameraFlyDir = 1;
        [SerializeField] private float _packMoveSpeed;
        [SerializeField] private ShopSettings _shopSettings;

        public int CameraFlyDir => _cameraFlyDir;
        
        public GameObject GetLevelPrefab() => _preyPackPrefab;
        
        public float PackMoveSpeed => _packMoveSpeed;
        
        public LevelEnvironment Environment => _environment;
        
        public List<PreySettings> PreySettingsList => _preySettings;

    }
}