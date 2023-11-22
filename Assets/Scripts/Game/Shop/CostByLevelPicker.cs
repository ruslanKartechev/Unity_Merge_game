using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    [System.Serializable]
    public class CostByLevelPicker
    {
        public List<CostByLevel> costs;

        public float GetCost(int level)
        {
            for (var i = costs.Count - 1; i >= 0; i--)
            {
                if (level >= costs[i].level)
                {
                    Debug.Log($"[COST PICKER] Level {level} cost: {costs[i].cost}");
                    return costs[i].cost;
                }
            }
            return costs[^1].cost;
        }
    }
}