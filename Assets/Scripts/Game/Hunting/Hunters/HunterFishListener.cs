using UnityEngine;

namespace Game.Hunting
{
    public class HunterFishListener : HunterListener
    {
        [SerializeField] private ParticleSystem _idleParticles;
        [SerializeField] private ParticleSystem _jumpStartParticles;
        [SerializeField] private ParticleSystem _trailParticles;
        [SerializeField] private ParticleSystem _splashPartciles;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        private const float HitForwardOffset = 0f; //2.55f;
        
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
            // var particles = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.GroundHitPunch),
            //     transform.position + transform.forward,
            //     Quaternion.identity);
        }

        public override void OnHitEnemy()
        {
            _splashPartciles.transform.parent = null;
            _splashPartciles.transform.position = _positionAdjuster.GetAdjustedPosition(transform.position 
                + transform.forward * HitForwardOffset);
            _splashPartciles.transform.rotation = Quaternion.identity;
            
            _splashPartciles.gameObject.SetActive(true);
            _splashPartciles.Play();            
        }
    }
}