using System.Collections;
using Common;
using DG.Tweening;
using Game.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Hunting
{
    public class PreyHumanDamageEffect : MonoBehaviour, IPreyDamageEffect
    {
        [SerializeField] private Transform _scalable;
        [SerializeField] private DamagedEffectSettings _settings;
        [SerializeField] private RendererColorer _colorer;

        private Coroutine _damaged;
                
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_colorer == null)
            {
                _colorer = gameObject.GetComponent<RendererColorer>();
                EditorUtility.SetDirty(this);
            }

            if (_scalable == null)
                _scalable = transform;
        }
#endif
        
        public void Damaged()
        {
            _scalable.localScale = Vector3.one * (1 + _settings._scaleMagn);
            _scalable.DOScale(Vector3.one, _settings._scaleTime).SetEase(_settings._scaleEase);
            StopDamagedColorSetting();
            _damaged = StartCoroutine(DamagedColorSetting());
        }

        public void Particles(Vector3 position)
        {
            var bloodFX = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.PreyBlood), position, Quaternion.identity);
            bloodFX.Play();
        }

        private void StopDamagedColorSetting()
        {
            if(_damaged != null)
                StopCoroutine(_damaged);
        }
        
        private IEnumerator DamagedColorSetting()
        {
            _colorer.SetColor(_settings._damagedColor);
            yield return new WaitForSeconds(_settings._damagedDuration);
            _colorer.SetColor(Color.white);
        }
    }
}