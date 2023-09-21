using UnityEngine;

namespace Game.Hunting
{
    public class TreeAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void Stop()
        {
            _animator.enabled = false;
        }
    }
}