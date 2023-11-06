using System;
using UnityEngine;

namespace Game.WorldMap
{
    public struct SpawnLevelArgs
    {
        public SpawnLevelArgs(int index, bool dead)
        {
            Index = index;
            Dead = dead;
        }

        public int Index;
        public bool Dead;
        
    }
    
    
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

    public class AnimateArgs
    {
        public Action OnComplete;
        public Action OnEnemyHidden;
        public float ScaleDuration;
        public float FadeDuration;
    }
}