using UnityEngine;

namespace Game.Hunting
{
    
    [CreateAssetMenu(menuName = "SO/" + nameof(PreySettings), fileName = nameof(PreySettings), order = 0)]
    public class PreySettings :  ScriptableObject, IPreySettings
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _health;
        [SerializeField] private float _reward;

        public float Health => _health;
        public float Reward => _reward;
        public float RotSpeed => _rotationSpeed;
    }
}