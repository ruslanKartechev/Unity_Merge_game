using System.Collections;
using Common;
using DG.Tweening;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Hunting
{
    public class PreyDamageEffect : MonoBehaviour, IPreyDamageEffect
    {
        [SerializeField] private Transform _scalable;
        [SerializeField] private float _scaleMagn;
        [SerializeField] private float _scaleTime;
        [SerializeField] private Ease _scaleEase;
        [Space(10)] 
        [SerializeField] private float _damagedDuration;
        [SerializeField] private Color _damagedColor;
        [Space(10)]
        [SerializeField] private Color _deadColor;
        [SerializeField] private float _delayBeforeDead;
        [SerializeField] private float _deadFadeTime;
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
        
        public void PlayDamaged()
        {
            _scalable.localScale = Vector3.one * (1 + _scaleMagn);
            _scalable.DOScale(Vector3.one, _scaleTime).SetEase(_scaleEase);
            StopDamagedColorSetting();
            _damaged = StartCoroutine(DamagedColorSetting());
        }

        private void StopDamagedColorSetting()
        {
            if(_damaged != null)
                StopCoroutine(_damaged);
        }

        public void PlayDead()
        {
            StopDamagedColorSetting();
            _damaged = StartCoroutine(DeadColorSettings());
        }

        private IEnumerator DeadColorSettings()
        {
            yield return new WaitForSeconds(_delayBeforeDead);
            _colorer.FadeToColor(_deadColor, _deadFadeTime);
        }
        private IEnumerator DamagedColorSetting()
        {
            _colorer.SetColor(_damagedColor);
            yield return new WaitForSeconds(_damagedDuration);
            _colorer.SetColor(Color.white);
        }
    }
}