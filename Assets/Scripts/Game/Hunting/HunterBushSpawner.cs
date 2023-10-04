using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterBushSpawner : MonoBehaviour
    {
        [SerializeField] private List<HuntersBush> _prefabs;
        private HuntersBush _spawned;
        public HuntersBush Bush => _spawned;

        public HuntersBush SpawnBush(Vector3 position, Quaternion rotation)
        {
            var prefab = _prefabs[0];
            var instance = Instantiate(prefab,  position, rotation, null);
            _spawned = instance;
            return _spawned;
        }
    }
}