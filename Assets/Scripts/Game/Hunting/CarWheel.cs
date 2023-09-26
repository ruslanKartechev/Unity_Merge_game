using System;
using UnityEngine;

namespace Game.Hunting
{
    public class CarWheel : MonoBehaviour
    {
        [SerializeField] private float _rotatedAngleR;
        [SerializeField] private float _rotatedAngleL;
        [SerializeField] private float _straightAngle;
        [SerializeField] private Transform _rotatable;
        [SerializeField] private float _defaultRotSpeed;

        private float _rotSpeed;
        public float RotationSpeed
        {
            get => _rotSpeed;
            set => _rotSpeed = value;
        }

        private void Awake()
        {
            RotationSpeed = _defaultRotSpeed;
        }

        public void RotateSide(float t, SidewaysDir dir = SidewaysDir.Right)
        {
            var rot = _rotatable.localRotation;
            var angle = 0f;
            if (dir == SidewaysDir.Right)
                angle = Mathf.Lerp(_straightAngle, _rotatedAngleR, t);
            else
                angle = Mathf.Lerp(_straightAngle, _rotatedAngleL, t);
            rot = Quaternion.Euler(0f,angle, 0f);
            _rotatable.localRotation = rot;
        }

        public void RotateMoving()
        {
            var rot = _rotatable.localRotation;
            rot *= Quaternion.Euler(-Time.deltaTime * RotationSpeed,0f,0f);
            _rotatable.localRotation = rot;
        }
        
    }
}