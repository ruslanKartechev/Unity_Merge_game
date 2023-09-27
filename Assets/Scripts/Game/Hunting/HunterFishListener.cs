using UnityEngine;

namespace Game.Hunting
{
    public class HunterFishListener : HunterListener
    {
        [SerializeField] private ParticleSystem _idleParticles;
        [SerializeField] private ParticleSystem _jumpStartParticles;
        [SerializeField] private ParticleSystem _trailParticles;
        [SerializeField] private ParticleSystem _splashPartciles;

        public override void OnAttack()
        {
            _idleParticles.gameObject.SetActive(false);

            _jumpStartParticles.transform.parent = null;
            _jumpStartParticles.gameObject.SetActive(true);
            _jumpStartParticles.Play();
            
            _trailParticles.gameObject.SetActive(true);
            _trailParticles.Play();
        }

        public override void OnFall()
        {
            _trailParticles.gameObject.SetActive(false);
            _idleParticles.gameObject.SetActive(false);
        }

        public override void OnBite()
        {
            _splashPartciles.gameObject.SetActive(true);
            _splashPartciles.Play();            
        }
    }
}