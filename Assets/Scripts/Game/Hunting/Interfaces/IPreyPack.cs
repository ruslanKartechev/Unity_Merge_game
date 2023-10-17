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
        event Action OnBeganMoving;

        int PreyCount { get; }
        ICamFollowTarget CamTarget { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        
        
        void Init(MovementTracks track, ILevelSettings levelSettings);
        void Idle();
        void RunAttacked();
        Vector3 LocalToWorld(Vector3 position);
        void RunCameraAround(CamFollower cam, Action returnCamera);
        float TotalPower();
    }
}