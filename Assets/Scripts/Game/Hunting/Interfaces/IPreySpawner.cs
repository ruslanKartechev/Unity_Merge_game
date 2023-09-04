using Dreamteck.Splines;

namespace Game.Hunting
{
    public interface IPreySpawner
    {
        IPrey Spawn(SplineComputer spline, ILevelSettings levelSettings);
    }
}