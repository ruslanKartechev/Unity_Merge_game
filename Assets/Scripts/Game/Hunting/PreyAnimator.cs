using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyAnimator : MonoBehaviour
    {
        // private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
        // private static readonly int RunOffset = Animator.StringToHash("RunOffset");
        // private static readonly int SurprisedKey = Animator.StringToHash("Surprised");

        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        [SerializeField] private string _injuredKey;
        [Space(10)] 
        [SerializeField] private Vector2 _runOffsetLimits;
        [Space(10)]
        [SerializeField] private List<AnimatorOverrideController> _controllerOverrides;
        [SerializeField] private float _healthPercentToInjure = .5f;
        private bool _wasDamaged;


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

        public void SetRunAnimationSpeed(float speed)
        {
            _animator.SetFloat("RunSpeed", speed);
        }
        
        public void RandomIdle()
        {
            RandomizeController();
            _animator.SetFloat("AnimationOffset", _runOffsetLimits.Random());
            _animator.Play(_idleKey);
        }

        public void PlayByName(string animName)
        {
            _animator.Play(animName);
        }
        
        public void TriggerByName(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        
        public void Run()
        {
            RandomizeController();
            _animator.SetFloat("AnimationOffset", _runOffsetLimits.Random());
            _animator.SetTrigger(_runKey);
        }

        public void Surprise()
        {
            _animator.SetTrigger("Surprised");
        }
        
        public void Injured(float health)
        {
            if (_wasDamaged)
                return;
            if (health <= _healthPercentToInjure)
            {
                _wasDamaged = true;
                _animator.SetTrigger(_injuredKey);
            }
        }
        
        private void RandomizeController()
        {
            _animator.runtimeAnimatorController = _controllerOverrides.Random();
        }

    }
}