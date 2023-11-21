using System;
using Game.Hunting.HuntCamera;

namespace Game.Hunting.Prey.Interfaces
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        void Init();
        void OnPackRun();
        void OnPackAttacked();
        float GetReward();
        PreySettings PreySettings { get; set; }
        ICamFollowTarget CamTarget { get; }
        bool IsAvailableTarget { get; }
    }
}