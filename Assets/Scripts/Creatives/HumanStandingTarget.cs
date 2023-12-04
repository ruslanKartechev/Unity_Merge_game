using System;
using Common.Ragdoll;
using UnityEngine;

namespace Creatives
{
    public class HumanStandingTarget : MonoBehaviour
    {
        [SerializeField] private float _pushForce = 20f;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _key;
        [SerializeField] private Collider _collider;
        [SerializeField] private string _collideTag;

        private void Start()
        {
            _animator.Play(_key);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_collideTag))
            {
                Die();   
                Push(other.transform);
            }
        }

        public void Push(Transform tr)
        {
            var vec = transform.position - tr.position;
            vec.y = 0f;
            _ragdoll.ActivateAndPush(vec * _pushForce);
        }

        public void Die()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }
            if (_animator != null)
            {
                _animator.enabled = false;
            }

            if (_ragdoll != null)
            {
                _ragdoll.Activate();
            }
        }
    }
}