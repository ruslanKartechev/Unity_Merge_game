using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Hunting
{
    public class IdleEnvironmentConcealer : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private float _moveTime;
        [SerializeField] private Ease _ease;


        public void HideAll()
        {
            foreach (var tt in _targets)
            {
                var posY = tt.position.y - 5f;
                var go = tt.gameObject;
                tt.DOMoveY(posY, _moveTime).SetEase(_ease).OnComplete(() => go.SetActive(false));
            }
        }
    }
}