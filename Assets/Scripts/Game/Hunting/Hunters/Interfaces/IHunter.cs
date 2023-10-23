using System;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting.Hunters.Interfaces
{
    public interface IHunter
    {
        public event Action<IHunter> OnDead;
        
        IHunterSettings Settings { get; }
        HunterAimSettings AimSettings { get; }
        ICamFollowTarget CameraPoint { get; }
        CamFollower CamFollower { get; set; }

        void Init(string item_id, MovementTracks track);
        void Run();
        void Idle();
        void Celebrate();
        
        void Jump(AimPath path);
        void RotateTo(Vector3 point);
        
        Transform GetTransform();
    }
    
}