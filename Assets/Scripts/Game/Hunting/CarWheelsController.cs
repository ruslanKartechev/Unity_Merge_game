using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public enum SidewaysDir {Right, Left}

    public class CarWheelsController : MonoBehaviour
    {
        [SerializeField] private List<CarWheel> _frontWheels;
        [Space(5)]
        [SerializeField] private List<CarWheel> _rearWheels;
        [Space(10)] 
        [SerializeField] private float _sidewaysRotSpeed = 5f;
        [SerializeField] private float _movingRotSpeed = 20f;

        private Coroutine _rotatingSideways;
        private Coroutine _mainRotation;
        private bool _isMoving;
        private bool _isRotatingSideways;


        private float _currentSpeed;
        public float CurrentSpeed
        {
            get => _currentSpeed;
            set => _currentSpeed = value;
        }
        
        public void RotateToRightAndBack(float time)
        {
            StopSidewaysRot();
            _rotatingSideways = StartCoroutine(RotatingSideAndBack(SidewaysDir.Right, time));
        }
        
        public void RotateToLeftAndBack(float time)
        {
            StopSidewaysRot();
            _rotatingSideways = StartCoroutine(RotatingSideAndBack(SidewaysDir.Left, time));
        }

        public void StartMoving()
        {
            
            StopMovingRotation();
            _isMoving = true;
            _mainRotation = StartCoroutine(MovingRotation());   
        }

        public void StopAll()
        {
            StopMovingRotation();
            StopSidewaysRot();
        }

        private void SetCurrentSpeed()
        {
            foreach (var fw in _frontWheels)
                fw.RotationSpeed = CurrentSpeed;
            foreach (var rw in _rearWheels)
                rw.RotationSpeed = CurrentSpeed;
        }

        private void StopMovingRotation()
        {
            if(_mainRotation != null)
                StopCoroutine(_mainRotation);
        }

        private void StopSidewaysRot()
        {
            if( _rotatingSideways != null)
                StopCoroutine(_rotatingSideways);
            _isRotatingSideways = false;
        }

        private IEnumerator MovingRotation()
        {
            CurrentSpeed = _movingRotSpeed;
            SetCurrentSpeed();
            while (true)
            {
                foreach (var fw in _frontWheels)
                    fw.RotateMoving();
                foreach (var fw in _rearWheels)
                    fw.RotateMoving();
                yield return null;
            }
        }
        
        // 1 => right, -1 => left
        private IEnumerator RotatingSideAndBack(SidewaysDir dir, float time)
        {
            if (_isMoving == false)
            {
                CurrentSpeed = _sidewaysRotSpeed;
                SetCurrentSpeed();
            }
            _isRotatingSideways = true;
            var elapsed = 0f;
            var tt = time / 2;
            while (elapsed <= tt)
            {
                var t = elapsed / tt;
                foreach (var ww in _frontWheels)
                {
                    ww.RotateSide(t, dir);
                    if (_isMoving == false)
                        ww.RotateMoving();
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = tt;
            while (elapsed >= 0)
            {
                var t = elapsed / tt;
                foreach (var ww in _frontWheels)
                {
                    ww.RotateSide(t, dir);
                    if (_isMoving == false)
                        ww.RotateMoving();
                }
                elapsed -= Time.deltaTime;
                yield return null;
            }
            _isRotatingSideways = false;
        }
    }
}