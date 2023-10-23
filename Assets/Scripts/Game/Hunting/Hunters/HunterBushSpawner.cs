using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterBushSpawner : MonoBehaviour
    {
        [SerializeField] private List<Data> _prefabData;
        private HuntersBush _spawned;
        
        
        public HuntersBush SpawnBush(Vector3 position, Quaternion rotation)
        {
            var env = GC.LevelRepository.GetLevel(GC.PlayerData.CurrentEnvironmentIndex).Environment;
            var prefab = _prefabData.Find(t => t.Environment == env);
           
            var instance = Instantiate(prefab.Prefab,  position, rotation, null);
            _spawned = instance;
            return _spawned;
        }
        
        
        [System.Serializable]
        public class Data
        {
            public LevelEnvironment Environment;
            public HuntersBush Prefab;
        }
    }
}