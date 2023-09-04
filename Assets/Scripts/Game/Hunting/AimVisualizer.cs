using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class AimVisualizer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fromParticles;
        [SerializeField] private ParticleSystem _toParticles;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _pointsCount;
        private List<Vector3> _positions;
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
        }
    }
}