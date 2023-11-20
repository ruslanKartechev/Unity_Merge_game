using System.Collections.Generic;

namespace Game.Hunting.Prey
{
    public interface IPreyAnimationKeys
    {
        IList<string> IdleAnimKeys { get; }
        IList<string> ScaredAnimKeys { get; }
        IList<string> WinAnimKeys { get; }
        string TreeCutAnimKey { get; }
        string RunTriggerKey { get; }
    }
}