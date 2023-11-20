using Game;
using Game.Core;
using UnityEngine;

namespace Common.SlowMotion
{
    [CreateAssetMenu(menuName = "SO/" + nameof(SlowMotionEffectSO), fileName = nameof(SlowMotionEffectSO), order = 0)]
    public class SlowMotionEffectSO : ScriptableObject
    {
        [SerializeField] private SlowMotionEffect _effect;
        public SlowMotionEffect Effect => _effect;

        public void Begin()
        {
            GC.SlowMotion.Begin(Effect);
        }

        public void Stop()
        {
            GC.SlowMotion.Exit(Effect);
        }
    }
}