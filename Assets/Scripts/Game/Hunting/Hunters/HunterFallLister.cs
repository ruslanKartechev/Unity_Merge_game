using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterFallLister : HunterListener 
    {
        [SerializeField] private RendererColorer _colorer;
        [SerializeField] private DamagedEffectSettings _settings;
        
        
        public override void OnAttack()
        { }

        public override void OnFall()
        {
            var particles = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.GroundHit), 
                transform.position, Quaternion.identity);
            // particles.Play();
            
            var particles2 = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.GroundHitPunch), 
                transform.position, Quaternion.identity);
            // particles2.Play();
            
            _colorer.FadeToColor(_settings._deadColor, _settings._deadFadeTime);
        }

        public override void OnHitEnemy()
        { }
    }
}