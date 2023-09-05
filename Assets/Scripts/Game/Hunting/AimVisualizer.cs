using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class AimVisualizer : MonoBehaviour
    {
        [SerializeField] private float _alphaOffset = 0.2f;
        [SerializeField] private float _distanceMin;
        [SerializeField] private float _distanceMax;
        [SerializeField] private Color _colorMin;
        [SerializeField] private Color _colorMax;
        
        [SerializeField] private ParticleSystem _fromParticles;
        [SerializeField] private ParticleSystem _toParticles;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _pointsCount;
        private AimPath _path;
        
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
            for (var i = 0; i < _pointsCount; i++)
            {
                var t = (float)i / (_pointsCount - 1);
                var pos = Bezier.GetPosition(_path.start, _path.inflection, _path.end, t);
                _lineRenderer.SetPosition(i, pos);
            }
            _fromParticles.transform.position = _path.start;
            _toParticles.transform.position = _path.end;
            var distance = (_path.end - _path.start).magnitude;
            var lerpVal = Mathf.InverseLerp(_distanceMin, _distanceMax, distance);
            var color = Color.Lerp(_colorMin, _colorMax, lerpVal);
            var gradient = new Gradient();
            var colorKeys = new GradientColorKey[1]
            {
                new(color, 0)
            };
            var alphaKeys = new GradientAlphaKey[4]
            {
                new(0, 0),
                new(1, _alphaOffset),
                new(1, 1 - _alphaOffset),
                new(0, 1)
            };
            gradient.SetKeys(colorKeys, alphaKeys);
            _lineRenderer.colorGradient = gradient;
        }
    }
}