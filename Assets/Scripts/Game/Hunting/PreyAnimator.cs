using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyAnimator : MonoBehaviour
    {
        private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleKey;
        [SerializeField] private string _runKey;
        [Space(10)] 
        [SerializeField] private List<AnimatorOverrideController> _controllerOverrides;


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
            _animator.SetFloat(RunSpeed, speed);
        }
        
        public void Idle()
        {
            _animator.runtimeAnimatorController = _controllerOverrides.Random();
            _animator.Play(_idleKey);
            
        }

        public void Run()
        {
            _animator.Play(_runKey);
        }
        
    }
}