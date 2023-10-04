using UnityEngine;

namespace Game.Hunting
{
    [System.Serializable]
    public class HunterAimSettings
    {
        public Vector2 AimInflectionUpLimits;
        public float AimInflectionOffsetVisual;
        public float StartAimLength = 5f;
        public float SensetivityMultipler = 1f;
    }
}