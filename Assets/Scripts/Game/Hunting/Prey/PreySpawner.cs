using Game.Hunting.Prey.Interfaces;
using Game.Levels;
using UnityEngine;

namespace Game.Hunting.Prey
{
    // NOT USED ANYMORE
    public class PreySpawner : MonoBehaviour, IPreySpawner
    {
        [SerializeField] private Transform _spawnPoint;
        
        public IPreyPack Spawn(MovementTracks track, ILevelSettings levelSettings)
        {
            var prefab = levelSettings.GetLevelPrefab();
            var instance = Instantiate(prefab);
            instance.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            var prey = instance.GetComponent<IPreyPack>();
            prey.Init(track, levelSettings);
            return prey;
        }
        
    }
}