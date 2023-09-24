using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;

namespace Game.Hunting
{
    public interface IHunterPack
    {
        event Action OnAllWasted;
        void SetCamera(CamFollower camFollower);
        void FocusCamera(bool animated = true);
        void SetHunters(IList<IHunter> hunters);
        void SetPrey(IPreyPack prey);
        void IdleState();
        void Activate();
        void Win();
    }
}