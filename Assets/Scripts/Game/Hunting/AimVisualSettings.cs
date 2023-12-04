using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(AimVisualSettings), fileName = nameof(AimVisualSettings), order = 0)]
    public class AimVisualSettings : ScriptableObject
    {
        public float FadeInTime = .25f;
        public float AlphaOffset = 0.2f;
        public float ParticlesUpOffset = .1f;
        public float FadeDuration = 1f;
    }
}