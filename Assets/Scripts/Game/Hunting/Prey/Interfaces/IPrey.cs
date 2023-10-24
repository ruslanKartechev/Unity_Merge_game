using System;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        void Init();
        void OnPackRun();
        float GetReward();
        void OnPackAttacked();
        PreySettings PreySettings { get; set; }
    }
}