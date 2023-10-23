using Game.Merging;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HunterViewProvider), fileName = nameof(HunterViewProvider), order = 0)]
    public class HunterViewProvider : ScriptableObject, IHunterViewProvider
    {
        [SerializeField] private GameObject _prefab;
        
        public GameObject GetPrefab()
        {
            return _prefab;
        }
    }
}