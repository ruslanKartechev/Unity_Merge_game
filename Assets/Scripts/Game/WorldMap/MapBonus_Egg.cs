using DG.Tweening;
using UnityEngine;

namespace Game.WorldMap
{
    public class MapBonus_Egg : MapBonus
    {
        private const float HideTime = 1f;
        private const float ScaleUpTime = .25f;
        private const float ScaleUp = 1.1f;
        
        
        public override void Collect()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one * ScaleUp, ScaleUpTime));
            seq.Append(transform.DOScale(Vector3.zero, HideTime));
        }
    }
}