using Dreamteck.Splines;

namespace Game.Hunting
{
    public interface IPreyPackMover
    {
        void Init(SplineComputer spline);
        float Speed { get; set; }
        float Acceleration { get; set; }
        
        void BeginMoving();
        void StopMoving();
    }
}