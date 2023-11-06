using System;

namespace Game.WorldMap
{
    public class AnimateArgs
    {
        public Action OnComplete;
        public Action OnEnemyHidden;
        public float ScaleDuration;
        public float FadeDuration;
    }
}