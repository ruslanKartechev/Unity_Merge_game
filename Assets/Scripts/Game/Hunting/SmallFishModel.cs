using System;
using System.Collections;
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
        [SerializeField] private ParticleSystem _hitParticles;

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
            SetPhysicsMode();
            _rb.AddForce(force, ForceMode.VelocityChange);
            _rb.AddTorque(force, ForceMode.VelocityChange);
        }

        public void NoPhys()
        {
            _rb.isKinematic = true;
            _collider.enabled = true;
        }

        public void FlyToHit(Vector3 position, float duration)
        {
            StartCoroutine(Flying(position, duration));
        }

        private void SetPhysicsMode()
        {
            _animator.enabled = false;
            _rb.isKinematic = false;
            _collider.enabled = true;   
        }


        private IEnumerator Flying(Vector3 endPos, float time)
        {
            var tr = _rb.transform;
            var elapsed = 0f;
            var startPos = tr.position;
            while (elapsed <= time)
            {
                tr.position = Vector3.Lerp(startPos, endPos, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (_hitParticles != null)
            {
                _hitParticles.transform.parent = null;
                _hitParticles.Play();
            }
            SetPhysicsMode();
        }
        
        
    }
    
}