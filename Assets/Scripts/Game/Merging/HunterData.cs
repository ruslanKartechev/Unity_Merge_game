using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HunterData), fileName = nameof(HunterData), order = 0)]
    public class HunterData : ScriptableObject, IHunterData
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private HunterSettings _hunterSettings;
        public GameObject GetPrefab()
        {
            return _prefab;
        }

        public IHunterSettings GetSettings() => _hunterSettings;
    }
}