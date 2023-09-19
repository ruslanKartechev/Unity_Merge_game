using System;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        
        ICamFollowTarget CamTarget { get; }
        void Activate();
        Vector3 GetPosition();
        Quaternion GetRotation();
        float GetReward();
    }
}