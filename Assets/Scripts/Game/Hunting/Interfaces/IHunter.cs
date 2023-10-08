using System;
using Game.Hunting.HuntCamera;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public interface IHunter
    {
        public event Action<IHunter> OnDead;
        void Init(IHunterSettings settings);
        public IHunterSettings Settings { get; }
        void SetPrey(IPreyPack preyPack);
        void Run();
        void Idle();
        HunterAimSettings AimSettings { get; }
        ICamFollowTarget GetCameraPoint();
        Transform GetTransform();
        void Jump(AimPath path);
        void Celebrate();
        void RotateTo(Vector3 point);
        public CamFollower CamFollower { get; set; }
    }
    
}