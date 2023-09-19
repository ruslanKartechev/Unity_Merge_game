using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(AimVisualSettings), fileName = nameof(AimVisualSettings), order = 0)]
    public class AimVisualSettings : ScriptableObject
    {
        public float AlphaOffset = 0.2f;
        public float DistanceMin;
        public float DistanceMax;
        public float InflectionVerticalOffset = 1f;
        public Color ColorMin;
        public Color ColorMax;
    }
}