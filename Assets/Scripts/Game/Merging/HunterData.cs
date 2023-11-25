using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HunterData), fileName = nameof(HunterData), order = 0)]
    public class HunterData : ScriptableObject, IHunterData
    {
        [SerializeField] private HunterSettings _hunterSettings;
        public GameObject GetPrefab()
        {
            var path = $"Prefabs/GameHunters/{name}";
            Debug.Log($"Path {path}");
            var ss = Resources.Load<GameObject>(path);
            return ss;
        }

        public IHunterSettings GetSettings() => _hunterSettings;
    }
}