using Common;
using UnityEngine;

namespace Game.WorldMap
{
    public class MapAnimatedTree : MonoBehaviour, IMapAnimatedPiece
    {
        private float _scaleBounce = 2f;
        private float _scaleFinal = 1f;
        private float _scaleStart = 0f;
        
        private float _startScale;

        public void Prepare()
        {
            _startScale = transform.localScale.x;
        }
        
        public void Animate(float t)
        {
            var scale = Bezier.GetValue(_scaleStart, _scaleBounce, _scaleFinal, t) * _startScale;
            transform.localScale = Vector3.one * scale;
        }
    }
}