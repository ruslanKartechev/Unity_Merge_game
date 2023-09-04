using DG.Tweening;
using UnityEngine;

namespace Game.UI.Elements
{
    public class PunchAnimator : MonoBehaviour
    {
        [SerializeField] private float _punch = 0.02f;
        [SerializeField] private float _punchTime = 0.25f;
        [SerializeField] private Transform _scalable;
   
        public void PunchAnimate()
        {
            _scalable.DOKill();
            _scalable.localScale = Vector3.one;
            _scalable.DOPunchScale(Vector3.one * _punch, _punchTime);   
        }

    }
}