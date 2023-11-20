using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HuntersBush : MonoBehaviour
    {
        [SerializeField] private List<Transform> _bushes;
        [SerializeField] private float _hideTime;
        [SerializeField] private float _hideDelay;
        [SerializeField] private Ease _hideEase;

        public void Hide()
        {
            StartCoroutine(DelayedHide());   
        }

        private IEnumerator DelayedHide()
        {
            yield return new WaitForSeconds(_hideDelay);
            foreach (var tr in _bushes)
            {
                var target = tr;
                target.DOScale(Vector3.zero, _hideTime).SetEase(_hideEase).OnComplete(() =>
                {
                    target.gameObject.SetActive(false);
                });
            }
        }
    }
}