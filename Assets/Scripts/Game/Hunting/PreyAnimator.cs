using UnityEngine;

namespace Game.Hunting
{
    public class PreyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }
#endif

        public void Disable()
        {
            _animator.enabled = false;
        }

        public void Idle()
        {
            _animator.Play(_idleKey);
        }

        public void Run()
        {
            _animator.Play(_runKey);
        }
        
    }
}