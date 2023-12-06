﻿namespace Common
{
    public interface ICameraShaker
    {
        void Play(CameraShakeArgs args);
        void PlayDefault();
        void Stop();
    }
}