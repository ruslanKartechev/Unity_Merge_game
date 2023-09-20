using UnityEngine;

namespace Game.Merging
{
    public interface IHunterSettings
    {
        float Damage { get; }
        float JumpSpeed { get; }
        float BiteOffset { get; }
    }
}