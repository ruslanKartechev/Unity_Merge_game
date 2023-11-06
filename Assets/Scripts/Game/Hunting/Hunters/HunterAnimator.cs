using UnityEngine;

namespace Game.Hunting
{
    public class HunterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        [SerializeField] private string _jumpKey;
        [SerializeField] private string _startKey;
        private static readonly int LandTrigger = Animator.StringToHash("Land");
        private static readonly int PrepareAttack = Animator.StringToHash("Prepare");


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }
#endif

        public void Idle()
        {
            _animator.Play(_idleKey);
        }

        public void Run()
        {
            _animator.Play(_runKey);
        }

        public void Jump()
        {
            _animator.Play(_jumpKey);
        }

        public void StartAnimation()
        {
            _animator.Play(_startKey);   
        }

        public void Land()
        {
            _animator.SetTrigger(LandTrigger);
        }

        public void Prepare()
        {
            _animator.Play(PrepareAttack);
        }
        
        public void Disable() => _animator.enabled = false;
    }
}