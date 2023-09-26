﻿namespace Common.SlowMotion
{
    public interface ISlowMotionManager
    {
        void Begin(SlowMotionEffect effect);
        void Exit(SlowMotionEffect effect);
        
        void SetNormalTime();
    }
}