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
        void SetPrey(IPrey prey);
        void Run();
        ICamFollowTarget GetCameraPoint();
        Transform GetTransform();
        void Jump(AimPath path);
        void Celebrate();
    }
}