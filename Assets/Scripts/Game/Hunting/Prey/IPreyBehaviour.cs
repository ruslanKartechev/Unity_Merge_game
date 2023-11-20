using System;

namespace Game.Hunting.Prey
{
    public interface IPreyBehaviour
    {
        void Begin();
        void Stop();
        event Action OnEnded;
    }
}