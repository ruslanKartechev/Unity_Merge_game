using System;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        
        ICamFollowTarget CamTarget { get; }
        void IdleState();
        void RunState();
        float GetReward();
        void SurpriseToAttack();

    }
}