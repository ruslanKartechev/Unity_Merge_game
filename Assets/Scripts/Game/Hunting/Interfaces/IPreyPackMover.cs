using Dreamteck.Splines;

namespace Game.Hunting
{
    public interface IPreyPackMover
    {
        void Init(MovementTracks track);
        
        void BeginMoving();
        void StopMoving();
    }
}