﻿using Common;
using UnityEngine;

namespace Game.WorldMap
{
    public class MapAnimatedProp : MonoBehaviour, IMapAnimatedPiece
    {
        private float _scaleBounce = 1.8f;
        private float _scaleFinal = 0f;
        private float _scaleStart = 1f;
        private float _rotationAngle = -60;
        private Quaternion _startRot; 
        private Quaternion _endRot;
        private float _startScale;
        
        public void Prepare()
        {
            _startRot = transform.rotation;
            _endRot = Quaternion.Euler(0f, _rotationAngle, 0f) * _startRot;
            _startScale = transform.localScale.x;
        }
        
        public void Animate(float t)
        {
            if (t > .5f)
            {
                transform.rotation = Quaternion.Lerp(_startRot, _endRot, 2f * t - 1f);
            }
            
            var scale = Bezier.GetValue(_scaleStart, _scaleBounce, _scaleFinal, t) * _startScale;
            transform.localScale = Vector3.one * scale;
        }
    }
}