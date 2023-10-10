using UnityEngine;

namespace Game.Hunting
{
    public class CarDamagedEffect : MonoBehaviour, IPreyDamageEffect
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _damagedAnim;

        public void Damaged()
        {
            _animator.Play(_damagedAnim);
        }

        public void Particles(Vector3 position)
        {
            var bloodFX = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.CarHit), position, Quaternion.identity);
            bloodFX.Play();
        }
    }
}