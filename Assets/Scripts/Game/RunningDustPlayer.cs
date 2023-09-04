using UnityEngine;

namespace Game
{
    public class RunningDustPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        
        
        public void ActivateDust()
        {
            _particle.Play();
        }
    }
}