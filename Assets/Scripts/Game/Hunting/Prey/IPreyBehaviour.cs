using System;

namespace Game.Hunting
{
    public interface IPreyBehaviour
    {
        void Begin();
        void Stop();
        event Action OnEnded;
    }
}