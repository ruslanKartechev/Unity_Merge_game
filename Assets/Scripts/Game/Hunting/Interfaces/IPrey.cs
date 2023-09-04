using System;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled; 
        void Activate();
        void Init(IPreySettings settings, SplineComputer path);
        Vector3 GetPosition();
        Quaternion GetRotation();
        ICamFollowTarget GetCameraPoint();
        float GetReward();
    }
}