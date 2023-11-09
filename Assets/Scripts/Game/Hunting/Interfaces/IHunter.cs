using System;
using Game.Hunting.HuntCamera;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public interface IHunter
    {
        public event Action<IHunter> OnDead;
        void Init(IHunterSettings settings, MovementTracks track);
        public IHunterSettings Settings { get; }
        HunterAimSettings AimSettings { get; }
        public CamFollower CamFollower { get; set; }
        public CamFollowTarget CameraPoint { get; }
        
        void SetPrey(IPreyPack preyPack);
        void Run();
        void Idle();
  
        ICamFollowTarget GetCameraPoint();
        Transform GetTransform();
        void Jump(AimPath path);
        void Celebrate();
        void RotateTo(Vector3 point);

    }
    
}