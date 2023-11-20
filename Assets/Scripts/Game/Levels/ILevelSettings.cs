using System.Collections.Generic;
using Game.Hunting;
using Game.Hunting.Prey;
using UnityEngine;

namespace Game.Levels
{
    public interface ILevelSettings
    {
        GameObject GetLevelPrefab();
        int CameraFlyDir { get; }
        LevelEnvironment Environment { get; }
        List<PreySettings> PreySettingsList { get; }
        LevelBonus Bonus { get; }
    }
}