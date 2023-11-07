using System;
using Game.Hunting.HuntCamera;

namespace Game.Hunting
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
    }
}