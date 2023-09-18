using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(100)]
    public class PreyPackMover : MonoBehaviour, IPreyPackMover
    {
        [SerializeField] private SplineFollower _splineFollower;
        private Coroutine _moving;
        private IPreySettings _settings;
        private SplineComputer _spline;
        private float _speed;
        
        public void Init(float moveSpeed, SplineComputer spline)
        {
            _speed = moveSpeed;
            _spline = spline;
            _splineFollower.spline = _spline;
            _splineFollower.followSpeed = moveSpeed;
            _splineFollower.enabled = true;
        }

        public void BeginMoving()
        {
            StopMoving();
            _splineFollower.enabled = true;
            _splineFollower.follow = true;
        }

        public void StopMoving()
        {
            _splineFollower.follow = false;
        }
        
    }
}