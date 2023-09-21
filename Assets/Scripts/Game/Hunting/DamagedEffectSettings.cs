using DG.Tweening;
using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(DamagedEffectSettings), fileName = nameof(DamagedEffectSettings), order = 0)]
    public class DamagedEffectSettings : ScriptableObject
    {
        public float _scaleMagn;
        public float _scaleTime;
        public Ease _scaleEase;
        [Space(10)] 
        public float _damagedDuration;
        public Color _damagedColor;
        [Space(10)]
        public Color _deadColor;
        public float _delayBeforeDead;
        public float _deadFadeTime;
    }
}