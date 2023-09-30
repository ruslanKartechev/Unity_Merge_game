using System;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPreyPack
    {
        event Action OnAllDead;
        event Action<IPrey> OnPreyKilled;
        event Action OnPreyChaseBegin; 

        int PreyCount { get; }
        void Idle();
        void RunAttacked();
        void Init(SplineComputer spline);
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 LocalToWorld(Vector3 position);
        ICamFollowTarget CamTarget { get; }
        ICamFollowTarget AttackCamTarget { get; }
        void RunCameraAround(CamFollower cam, Action returnCamera);

    }
}