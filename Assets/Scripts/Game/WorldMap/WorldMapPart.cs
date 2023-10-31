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
        public abstract void HideLevel();

        public abstract void FogSetActive(bool active);
        public abstract void GlowSetActive(bool active);
        
        
        public abstract void SetEnemyTerritory();
        public abstract void SetPlayerTerritory();
        
        
    }
}