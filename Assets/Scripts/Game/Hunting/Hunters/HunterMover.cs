using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterMover : MonoBehaviour
    {
        [SerializeField] private SplineFollower _splineFollower;
        private Coroutine _moving;
        private float _speed;
        private float _targetSpeed = 0f;
        private float _accelerationDuration;
        
        public float Speed
        {
            get => _splineFollower.followSpeed;
            set => _splineFollower.followSpeed = value;
        }

        
        public void SetSpline(MovementTracks track, SplineComputer spline)
        {
            _splineFollower.spline = spline;
            _splineFollower.follow = false;
            _splineFollower.followSpeed = _speed = 0f;
            _splineFollower.enabled = true;
            _targetSpeed = track.moveSpeed;
            _accelerationDuration = track.accelerationDuration;
            
            // Debug.Log($"Spline name: {spline.gameObject.name}");
            var sample = spline.Project(transform.position);
            var offset = transform.position - sample.position;
            // Debug.Log($"Offset: {offset}");
            _splineFollower.motion.offset = offset;
        }

        public void Move()
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