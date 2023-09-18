using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPreyMover
    {
        void Init(IPreySettings settings, SplineComputer spline);
        void BeginMoving();
        void StopMoving();
    }
    [DefaultExecutionOrder(100)]
    public class PreyMover : MonoBehaviour, IPreyMover
    {
        [SerializeField] private SplineFollower _splineFollower;
        private Coroutine _moving;
        private IPreySettings _settings;
        private SplineComputer _spline;
        
        public void Init(IPreySettings settings, SplineComputer spline)
        {
            _settings = settings;
            _spline = spline;
            _splineFollower.spline = _spline;
            _splineFollower.followSpeed = _settings.MoveSpeed;
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