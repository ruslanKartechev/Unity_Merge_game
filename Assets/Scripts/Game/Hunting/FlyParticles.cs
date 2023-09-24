using System;
using UnityEngine;

namespace Game.Hunting
{
    public class FlyParticles : MonoBehaviour
    {
        public static FlyParticles Instance;
        [SerializeField] private ParticleSystem _particles;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Multiple Instances of FlyParticles");
                Destroy(gameObject);
            }
            Instance = this;
        }

        public void Play()
        {
            _particles.Play();      
        }

        public void Stop()
        {
            _particles.Stop();
            _particles.Clear();
        }
    }
}