using System;

namespace Creatives.Office
{
    public interface ICreosHunter
    {
        event Action<ICreosHunter> OnDead;
        void SetActive();
    }
}