using UnityEngine;

namespace Game.WorldMap
{
    public abstract class WorldMapPart : MonoBehaviour
    {
        protected GameObject _levelInstance;

        public abstract WorldMapCameraPoint CameraPoint { get; set; }

        public abstract void Show();
        public abstract void Hide();
        
        public abstract void SpawnLevelEnemies(int index);
        public abstract void ShowLevelNumber(int level);
        public abstract void HideLevelNumber();
        
        public abstract void HideLevel();
        public abstract void SetEnemyTerritory();
        public abstract void SetPlayerTerritory();
        
        
    }
}