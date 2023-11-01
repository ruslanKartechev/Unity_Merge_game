using Game.UI.Map;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(10)]
    public class MapSceneManager : MonoBehaviour
    {
        [SerializeField] private MapCamera _camera;
        [SerializeField] private WorldMapManager _mapManager;
        
        public void Start()
        {
            var currentLevel = GC.PlayerData.LevelTotal;
            ShowLevelAsCaptured(currentLevel);
        }

        public void ShowNextLevel(int currentLevel)
        {
            _mapManager.ShowLevel(currentLevel);   
        }

        public void ShowLevelAsCaptured(int currentLevel)
        {
            _mapManager.AnimateToPlayer(currentLevel);
        }

      
    }
}