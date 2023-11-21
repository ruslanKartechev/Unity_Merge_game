using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HunterBushSpawner : MonoBehaviour
    {
        [SerializeField] private List<Data> _prefabData;
        private HuntersBush _spawned;
        
        public HuntersBush SpawnBush(Vector3 position, Quaternion rotation)
        {
            var data = _prefabData[GC.PlayerData.CurrentEnvironmentIndex];
            var instance = Instantiate(data.Prefab,  position, rotation, null);
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