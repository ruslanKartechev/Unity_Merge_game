using System;
using System.Collections.Generic;
using Common;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters.Interfaces
{
    public interface IHunterPack
    {
        event Action OnAllWasted;
        void Init(IPreyPack prey, ProperButton inputButton, GameObject camera, MovementTracks track);
        void FocusCamera(bool animated = true);
        void SetHunters(IList<IHunter> hunters);
        void IdleState();
        void BeginChase();
        void AllowAttack();
        void Win();
        float TotalPower();
    }
}