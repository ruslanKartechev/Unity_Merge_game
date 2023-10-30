using Game.UI.Map;
using UnityEngine;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(10)]
    public class MapSceneManager : MonoBehaviour
    {
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapManager _mapManager;
        [SerializeField] private MapLevelsDisplay _mapLevels;
        
        public void Start()
        {
            var currentLevel = GC.PlayerData.LevelTotal;
            _mapLevels.ShowLevel(currentLevel);
            _mapManager.ShowLevel(currentLevel);
        }
        
    }
}