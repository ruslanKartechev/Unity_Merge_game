using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;

namespace Game.Hunting
{
    public interface IHunterPack
    {
        event Action OnAllWasted;
        void Init(IPreyPack prey, CamFollower camFollower);
        void FocusCamera(bool animated = true);
        void SetHunters(IList<IHunter> hunters);
        void IdleState();
        void BeginChase();
        void AllowAttack();
        void Win();
    }
}