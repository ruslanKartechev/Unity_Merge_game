using Dreamteck.Splines;

namespace Game.Hunting
{
    public interface IPreyPackMover
    {
        void Init(float speed, SplineComputer spline);
        void BeginMoving();
        void StopMoving();
    }
}