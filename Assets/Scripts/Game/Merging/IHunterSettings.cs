using UnityEngine;

namespace Game.Merging
{
    public interface IHunterSettings
    {
        float Damage { get; }
        float JumpSpeed { get; }
        LayerMask BiteMask { get; }
        float BiteDistance { get; }        
        float BiteOffset { get; }
    }
}