﻿using System;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        void Init();
        void IdleState();
        void RunState();
        float GetReward();
        void SurpriseToAttack();

    }
}