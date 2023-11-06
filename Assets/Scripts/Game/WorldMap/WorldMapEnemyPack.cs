using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemyPack : MonoBehaviour
    {
        [SerializeField] private List<WorldMapEnemy> _mapEnemies;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _mapEnemies = Common.Utils.HierarchyUtils.GetFromAllChildren<WorldMapEnemy>(transform);
        }
#endif

        public void ShowActive()
        {
            foreach (var unit in _mapEnemies)
                unit.Active();
        }

        public void ShowDead()
        {
            foreach (var unit in _mapEnemies)
                unit.Dead();   
        }
        
    }
}