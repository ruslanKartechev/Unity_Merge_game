using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;

namespace Game.Levels
{
    public interface ILevel
    {
        void Init(IHuntUIPage ui, SplineComputer track, CamFollower camera);
    }
}