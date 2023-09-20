using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(100)]
    public class PreyPackMover : MonoBehaviour, IPreyPackMover
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _maxAcceleration;
        [SerializeField] private SplineFollower _splineFollower;
        
        private Coroutine _moving;
        private IPreySettings _settings;
        private SplineComputer _spline;
        private float _speed;
        private float _acceleration;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        public float Acceleration
        {
            get => _acceleration;
            set => _acceleration = value;
        }
        
        public void Init(SplineComputer spline)
        {
            Speed = 0;
            Acceleration = _maxAcceleration;
            _spline = spline;
            _splineFollower.followSpeed = 0;
            _splineFollower.spline = _spline;
            _splineFollower.enabled = true;
        }
        
        public void BeginMoving()
        {
            StopMoving();
            _splineFollower.enabled = true;
            _splineFollower.follow = true;
            AccelerateFromStart();
        }

        public void StopMoving()
        {
            _splineFollower.follow = false;
        }

        private void AccelerateFromStart()
        {
            StartCoroutine(Accelerating(_maxSpeed));
        }

        private IEnumerator Accelerating(float targetSpeed)
        {
            var startSpeed = Speed;
            var time = (_maxSpeed - Speed) / targetSpeed;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                Speed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / time);
                _splineFollower.followSpeed = Speed;
                elapsed += Time.deltaTime;
                yield return null;
            }
            Speed = targetSpeed;
            _splineFollower.followSpeed = Speed;
        }
    }
}