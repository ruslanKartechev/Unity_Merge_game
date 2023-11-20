using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class FishParticlesManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _idleParticles_land;
        [SerializeField] private ParticleSystem _idleParticles_water;
        [Space(10)]
        [SerializeField] private ParticleSystem _jumpStartParticles;
        [SerializeField] private ParticleSystem _trailParticles;
        [SerializeField] private ParticleSystem _splashPartciles;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        private const float HitForwardOffset = 0f; //2.55f;

        private ParticleSystem _idleParticles;
        
        public void IdleOnLand()
        {
            _idleParticles = _idleParticles_land;
            if (_idleParticles != null)
            {
                _idleParticles.gameObject.SetActive(true);
                _idleParticles.Play();
            }
        }

        public void IdleOnWater()
        {
            if(_idleParticles_land != null)
                _idleParticles_land.gameObject.SetActive(false);
            _idleParticles = _idleParticles_water;
            if (_idleParticles != null)
            {
                _idleParticles.gameObject.SetActive(true);
                _idleParticles.Play();              
            }
        }

        public void JumpAttack()
        {
            if(_idleParticles != null)
                _idleParticles.gameObject.SetActive(false);

            _jumpStartParticles.transform.parent = null;
            _jumpStartParticles.gameObject.SetActive(true);
            _jumpStartParticles.Play();
            
            _trailParticles.gameObject.SetActive(true);
            _trailParticles.Play();   
        }

        public void HitEnemy()
        {
            var tr = _splashPartciles.transform;
            tr.parent = null;
            tr.position = _positionAdjuster.GetAdjustedPosition(transform.position 
                + transform.forward * HitForwardOffset);
            tr.rotation = Quaternion.identity;
            
            _splashPartciles.gameObject.SetActive(true);
            _splashPartciles.Play();    
        }
        
    }
}