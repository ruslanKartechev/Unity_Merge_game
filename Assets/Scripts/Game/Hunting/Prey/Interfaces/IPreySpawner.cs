
using Game.Levels;

namespace Game.Hunting.Prey.Interfaces
{
    public interface IPreySpawner
    {
        IPreyPack Spawn(MovementTracks track, ILevelSettings levelSettings);
    }
}