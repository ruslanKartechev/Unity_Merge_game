using System.Collections;
using Dreamteck.Splines;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Prey
{
    [DefaultExecutionOrder(100)]
    public class PreyPackMover : MonoBehaviour, IPreyPackMover
    {
        [SerializeField] private SplineFollower _splineFollower;
        private Coroutine _moving;
        private float _speed;
        private float _targetSpeed = 0f;
        private float _accelerationDuration;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        
        public void Init(MovementTracks track)
        {
            _targetSpeed = track.moveSpeed;
            _accelerationDuration = track.accelerationDuration;
            _splineFollower.spline = track.main;
            _splineFollower.followSpeed = Speed = 0;
            _splineFollower.enabled = true;
            
            var sample = _splineFollower.spline.Project(transform.position);
            var offset = transform.position - sample.position;
            _splineFollower.motion.offset = offset;
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
            StartCoroutine(Accelerating(_targetSpeed, _accelerationDuration));
        }
        
        private IEnumerator Accelerating(float targetSpeed, float duration)
        {
            var startSpeed = Speed;
            var elapsed = 0f;
            while (elapsed <= duration)
            {
                Speed = Mathf.Lerp(startSpeed, targetSpeed, duration );
                _splineFollower.followSpeed = Speed;
                elapsed += Time.deltaTime;
                yield return null;
            }
            Speed = targetSpeed;
            _splineFollower.followSpeed = Speed;
        }
    }
}