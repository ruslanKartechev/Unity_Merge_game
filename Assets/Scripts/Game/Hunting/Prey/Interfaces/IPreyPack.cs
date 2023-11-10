using System;
using System.Collections.Generic;
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
        Vector3 Position { get; }
        ICamFollowTarget CamTarget { get; }
        HashSet<IPrey> GetPrey();

        void Init(MovementTracks track, ILevelSettings levelSettings);
        void Idle();
        void RunAttacked();
        void RunCameraAround(CamFollower cam, Action returnCamera);
        float TotalPower();
    }
}