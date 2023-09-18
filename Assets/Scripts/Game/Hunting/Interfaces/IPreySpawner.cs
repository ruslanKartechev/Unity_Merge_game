using Dreamteck.Splines;

namespace Game.Hunting
{
    public interface IPreySpawner
    {
        IPreyPack Spawn(SplineComputer spline, ILevelSettings levelSettings);
    }
}