using System;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class SmallFishModel : MonoBehaviour
    {
        [SerializeField] private Vector2 _animaionOffsetLimits;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;

        private void Start()
        {
            _animator.SetFloat("RandomOffset", _animaionOffsetLimits.Random());
        }

        public void Push(Vector3 force)
        {
            _animator.enabled = false;
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(force, ForceMode.VelocityChange);
            _rb.AddTorque(force, ForceMode.VelocityChange);
        }

 
    }
}