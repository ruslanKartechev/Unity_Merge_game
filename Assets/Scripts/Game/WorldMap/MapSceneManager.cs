using UnityEngine;

namespace Game.WorldMap
{
    public class MapSceneManager : MonoBehaviour
    {
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapManager _mapManager;

        public void Start()
        {
            var currentLevel = GC.PlayerData.LevelTotal;
            _mapManager.ShowLevel(currentLevel);
    
        }
        
        
    }
}