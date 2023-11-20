using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Prey
{
    [CreateAssetMenu(menuName = "SO/" + nameof(PreyData), fileName = nameof(PreyData), order = 0)]
    public class PreyData :  ScriptableObject, IPreyData
    {
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;
    }
}