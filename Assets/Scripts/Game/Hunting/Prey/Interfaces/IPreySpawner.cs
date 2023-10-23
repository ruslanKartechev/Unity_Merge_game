
namespace Game.Hunting
{
    public interface IPreySpawner
    {
        IPreyPack Spawn(MovementTracks track, ILevelSettings levelSettings);
    }
}