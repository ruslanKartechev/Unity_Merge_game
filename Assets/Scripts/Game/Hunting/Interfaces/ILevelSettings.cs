using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public interface ILevelSettings
    {
        GameObject GetLevelPrefab();
        int CameraFlyDir { get; }
        LevelEnvironment Environment { get; }
        List<PreySettings> PreySettingsList { get; }
    }
}