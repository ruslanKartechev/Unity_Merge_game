using System.Collections;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class AimVisualizer : MonoBehaviour
    {
        [SerializeField] private AimVisualSettings _settings;        
        [SerializeField] private ParticleSystem _fromParticles;
        [SerializeField] private ParticleSystem _toParticles;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _pointsCount;
        private AimPath _path;
        private Coroutine _fading;
        
        public float InflectionOffset { get; set; }
        
        public void Show(AimPath path)
        {
            _path = path;
            _lineRenderer.positionCount = _pointsCount;
            _lineRenderer.enabled = true;
            _fromParticles.Play();
            _toParticles.Play();
            StopFade();
            LerpFade(0);
        }

        public void Hide()
        {
            StopFade();
            _fading = StartCoroutine(Fading());
            // _lineRenderer.enabled = false;
            _fromParticles.Stop();
            _toParticles.Stop();
        }

        public void UpdatePath()
        {
            AssignEndPosition(out var endPos, out var endRot);
            var upOffset = Vector3.up * InflectionOffset;
            for (var i = 0; i < _pointsCount; i++)
            {
                var t = (float)i / (_pointsCount - 1);
                var pos = Bezier.GetPosition(_path.start, 
                    _path.inflection + upOffset,
                        endPos, t);
                _lineRenderer.SetPosition(i, pos);
            }

            _fromParticles.transform.position = _path.start + Vector3.up * _settings.ParticlesUpOffset;
            var topp = _toParticles.transform.parent;
            topp.position = endPos;
            topp.rotation = endRot;
        }

        private void AssignEndPosition(out Vector3 endPos, out Quaternion endRotation)
        {
            var rayDir = (_path.end - _path.inflection);
            if (Physics.Raycast(_path.inflection, rayDir, out var hit, 100, _settings.EnemyMask))
            {
                endPos = hit.point;
                endRotation = Quaternion.LookRotation(hit.normal);
                return;
            }
            endPos = _path.end + Vector3.up * _settings.ParticlesUpOffset;
            endRotation = Quaternion.LookRotation(Vector3.up);
        }

        private void StopFade()
        {
            if(_fading != null)
                StopCoroutine(_fading);
        }
        
        private IEnumerator Fading()
        {
            var elapsed = 0f;
            var time = _settings.FadeDuration;
            while (elapsed <= time)
            {
                LerpFade(elapsed / time);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            _lineRenderer.enabled = false;
        }
        
        private void LerpFade(float t)
        {
            var gradient = new Gradient();
            var colorKeys = new GradientColorKey[1]
            {
                new(_lineRenderer.colorGradient.colorKeys[0].color, 0)
            };
            var alpha = Mathf.Lerp(1f, 0f, t);
            var alphaKeys = new GradientAlphaKey[]
            {
                new(0, 0),
                new(alpha, _settings.AlphaOffset),
                new(alpha, 1)
            };
            gradient.SetKeys(colorKeys, alphaKeys);
            _lineRenderer.colorGradient = gradient;
        }
    }
}