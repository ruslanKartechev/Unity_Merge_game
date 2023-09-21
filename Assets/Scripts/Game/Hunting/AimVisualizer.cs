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
        
        public float InflectionOffset { get; set; }
        
        public void Show(AimPath path)
        {
            _path = path;
            _lineRenderer.positionCount = _pointsCount;
            _lineRenderer.enabled = true;
            _fromParticles.Play();
            _toParticles.Play();
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
            _fromParticles.Stop();
            _toParticles.Stop();
        }

        public void UpdatePath()
        {
            var upOffset = Vector3.up * InflectionOffset;
            for (var i = 0; i < _pointsCount; i++)
            {
                var t = (float)i / (_pointsCount - 1);
                var pos = Bezier.GetPosition(_path.start, 
                    _path.inflection + upOffset,
                        _path.end, 
                        t);
                _lineRenderer.SetPosition(i, pos);
            }
            _fromParticles.transform.position = _path.start;
            _toParticles.transform.position = _path.end;
            var distance = (_path.end - _path.start).magnitude;
            var lerpVal = Mathf.InverseLerp(_settings.DistanceMin, _settings.DistanceMax, distance);
            var color = Color.Lerp(_settings.ColorMin, _settings.ColorMax, lerpVal);
            var gradient = new Gradient();
            var colorKeys = new GradientColorKey[1]
            {
                new(color, 0)
            };
            var alphaKeys = new GradientAlphaKey[4]
            {
                new(0, 0),
                new(1, _settings.AlphaOffset),
                new(1, 1 - _settings.AlphaOffset),
                new(0, 1)
            };
            gradient.SetKeys(colorKeys, alphaKeys);
            _lineRenderer.colorGradient = gradient;
        }
    }
}