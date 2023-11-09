using System;
using Game.Hunting.HuntCamera;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        void Init();
        void IdleState();
        void RunState();
        float GetReward();
        void SurpriseToAttack();
        PreySettings PreySettings { get; set; }
        public CamFollowTarget CamTarget { get; }

    }
}