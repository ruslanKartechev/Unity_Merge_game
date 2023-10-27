using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldMap
{
    [CreateAssetMenu(menuName = "SO/" + nameof(WorldMapEnemyPacksRepository), fileName = nameof(WorldMapEnemyPacksRepository), order = 0)]
    public class WorldMapEnemyPacksRepository : ScriptableObject
    {
        [SerializeField] private GameObject _default;
        [SerializeField] private List<GameObject> _prefabs;
        

        public GameObject GetPrefab(int levelIndex)
        {
            if (levelIndex >= _prefabs.Count)
            {
                Debug.Log($"[WorldMap] {levelIndex} >= {_prefabs.Count}. Return default");
                return _default;
            }

            if (_prefabs[levelIndex] == null)
                return _default;
            return _prefabs[levelIndex];
        }
    }
}