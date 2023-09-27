using UnityEngine;

namespace Game.Hunting
{
    public interface ILevelSettings
    {
        GameObject GetPreyPack();
        int CameraFlyDir { get; }
        LevelEnvironment Environment { get; }
    }
}