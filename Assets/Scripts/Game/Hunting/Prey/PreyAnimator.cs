using UnityEngine;

namespace Game.Hunting
{
    public class PreyAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected string _injuredKey;
        [SerializeField] protected float _healthPercentToInjure = .5f;
        private bool _isInjuredAnim;
        private static readonly int AnimationOffset = Animator.StringToHash("AnimationOffset");


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

        public void SetOverride(RuntimeAnimatorController controller)
        {
            _animator.runtimeAnimatorController = controller;
        }
        
        public void SetRunAnimationSpeed(float speed)
        {
            _animator.SetFloat("RunSpeed", speed);
        }
        
        public void PlayByName(string animName)
        {
            // Debug.Log($"ByName {animName}");
            _animator.Play(animName);
        }
        
        public void PlayByName(string animName, float offset)
        {
            // Debug.Log($"ByName {animName}");
            _animator.SetFloat(AnimationOffset, offset);
            _animator.Play(animName);
        }
        
        public void PlayByTrigger(string triggerName)
        {
            // Debug.Log($"ByTrigger {triggerName}");
            _animator.SetTrigger(triggerName);
        }
        
        public void Injured(float health)
        {
            if (_isInjuredAnim)
                return;
            if (health <= _healthPercentToInjure)
            {
                _isInjuredAnim = true;
                _animator.SetTrigger(_injuredKey);
            }
        }

        public void ForceInjured()
        {
            _isInjuredAnim = true;
            _animator.SetTrigger(_injuredKey);
        }
    }
}