using UnityEngine;

namespace Game.Hunting
{
    public class CarDamagedEffect : MonoBehaviour, IPreyDamageEffect
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _damagedAnim;
        [SerializeField] private ParticleSystem _particles;

        public void PlayDamaged()
        {
            _animator.Play(_damagedAnim);
        }

        public void PlayAt(Vector3 position)
        {
            if (_particles == null)
                return;
            _particles.transform.position = position;
            _particles.Play();
        }
    }
}