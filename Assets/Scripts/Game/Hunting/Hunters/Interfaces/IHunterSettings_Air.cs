using UnityEngine;

namespace Game.Hunting.Hunters.Interfaces
{
    public interface IHunterSettings_Air : IHunterSettings
    {
        Vector3 FlyAwayOffset { get; }
        float FlyAwayDuration { get; }
        float LiftUpDuration { get; }
        float LiftUpHeight { get; }
    }
}