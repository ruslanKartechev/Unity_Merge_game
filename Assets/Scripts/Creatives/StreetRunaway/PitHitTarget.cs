using Creatives.Office;
using UnityEngine;

namespace Creatives.StreetRunaway
{
    public class PitHitTarget : MonoBehaviour, IHitTarget
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationKey;

        public void OnHit()
        {
            _animator.enabled = true;
            _animator.Play(_animationKey);
        }
        
    }
}