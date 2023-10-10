using Common.Data;
using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ParticlesRepository), fileName = nameof(ParticlesRepository), order = 0)]
    public class ParticlesRepository : ScriptableObject
    {
        [SerializeField] private DataByTypeRepository<ParticleSystem, EParticleType> _particlesData;

        public void Init()
        {
            _particlesData.Init();
        }
        public ParticleSystem GetParticles(EParticleType type) => _particlesData.GetData(type);
    }
}