using System;
using Game.Hunting.HuntCamera;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public interface IHunter
    {
        public event Action<IHunter> OnDead;
        void Init(IHunterSettings settings,  CamFollower camFollower);
        void SetPrey(IPreyPack preyPack);
        void Run();
        ICamFollowTarget GetCameraPoint();
        Transform GetTransform();
        void Jump(AimPath path);
        void Celebrate();
    }
}