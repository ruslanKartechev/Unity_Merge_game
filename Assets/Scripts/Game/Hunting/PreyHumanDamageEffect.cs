using System.Collections;
using Common;
using DG.Tweening;
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
        [SerializeField] private ParticleSystem _particles;

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
        
        public void PlayDamaged()
        {
            _scalable.localScale = Vector3.one * (1 + _settings._scaleMagn);
            _scalable.DOScale(Vector3.one, _settings._scaleTime).SetEase(_settings._scaleEase);
            StopDamagedColorSetting();
            _damaged = StartCoroutine(DamagedColorSetting());
        }

        public void PlayAt(Vector3 position)
        {
            if (_particles == null)
                return;
            _particles.transform.position = position;
            _particles.Play();
        }

        private void StopDamagedColorSetting()
        {
            if(_damaged != null)
                StopCoroutine(_damaged);
        }

        public void PlayDead()
        {
            _particles.Play();
            // StopDamagedColorSetting();
            // _damaged = StartCoroutine(DeadColorSettings());
        }

        private IEnumerator DeadColorSettings()
        {
            yield return new WaitForSeconds(_settings._delayBeforeDead);
            _colorer.FadeToColor(_settings._deadColor, _settings._deadFadeTime);
        }
        private IEnumerator DamagedColorSetting()
        {
            _colorer.SetColor(_settings._damagedColor);
            yield return new WaitForSeconds(_settings._damagedDuration);
            _colorer.SetColor(Color.white);
        }
    }
}