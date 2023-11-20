namespace Game.Hunting.Prey.Interfaces
{
    public interface IPreyPackMover
    {
        void Init(MovementTracks track);
        
        void BeginMoving();
        void StopMoving();
    }
}