using UnityEngine;

namespace Game.Merging
{
    public interface IHunterData
    {
        GameObject GetPrefab();
        public IHunterSettings GetSettings();
    }
}