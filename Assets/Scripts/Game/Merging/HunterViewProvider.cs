using UnityEngine;

namespace Game.Merging
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