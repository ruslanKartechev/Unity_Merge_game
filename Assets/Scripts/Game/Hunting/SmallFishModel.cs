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

        private float _scale;
        
        private void Start()
        {
            _animator.SetFloat("RandomOffset", _animaionOffsetLimits.Random());
            _scale = transform.localScale.x;
        }

        public void ScaleTo0(float t)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(_scale, 0f, t);
        }

        public void Push(Vector3 force)
        {
            _animator.enabled = false;
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(force, ForceMode.VelocityChange);
            _rb.AddTorque(force, ForceMode.VelocityChange);
        }

        public void NoPhys()
        {
            _rb.isKinematic = true;
            _collider.enabled = true;
        }
        
    }
}