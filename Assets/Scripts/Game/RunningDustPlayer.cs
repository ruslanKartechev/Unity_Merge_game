using UnityEngine;

namespace Game
{
    public class RunningDustPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSystem _particleLeft;
        [SerializeField] private ParticleSystem _particleRight;
        
        public void ActivateDust()
        {
            _particle.Play();
        }

        public void DustLeft()
        {
            _particleLeft.Play();   
        }

        public void DustRight()
        {
            _particleRight.Play();
        }
    }
}