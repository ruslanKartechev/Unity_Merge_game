﻿using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;

namespace Game.Hunting
{
    public interface IHunterPack
    {
        event Action OnAllWasted;
        void SetCamera(CamFollower camFollower);
        void SetHunters(IList<IHunter> hunters);
        void SetPrey(IPrey prey);
        void Activate();
        void Win();
    }
}