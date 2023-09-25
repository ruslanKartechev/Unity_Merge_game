using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterFallLister : HunterListener 
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private RendererColorer _colorer;
        [SerializeField] private DamagedEffectSettings _settings;
        public override void OnAttack()
        { }

        public override void OnFall()
        {
            _particles.gameObject.SetActive(true);
            _particles.Play();
            _colorer.FadeToColor(_settings._deadColor, _settings._deadFadeTime);
        }

        public override void OnBite()
        { }
    }
}