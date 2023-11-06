using UnityEngine;

namespace Game.WorldMap
{
    public abstract class WorldMapPart : MonoBehaviour
    {
        [SerializeField] protected Transform _playerSpawnPoint;
        protected GameObject _levelInstance;
        
        public virtual Transform PlayerSpawn => _playerSpawnPoint;
        public abstract WorldMapCameraPoint CameraPoint { get; set; }
        
        
        public abstract void Show();
        public abstract void Hide();
        
        public abstract void SpawnLevelEnemies(SpawnLevelArgs args);
        public abstract void HideLevel();

        public abstract void FogSetActive(bool active);
        public abstract void GlowSetActive(bool active);
        
        
        public abstract void SetEnemyTerritory();
        public abstract void SetPlayerTerritory();

        public abstract void AnimateToPlayer(AnimateArgs args);
        public abstract void ArrowSetActive(bool active);
    }
}