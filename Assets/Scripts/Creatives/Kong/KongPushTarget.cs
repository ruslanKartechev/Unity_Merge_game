using System;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongPushTarget : MonoBehaviour, IKongPushTarget
    {
        [SerializeField] private Transform _direction;
        [SerializeField] private float _force;
        [SerializeField] private float _torque;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private bool _animated = true;
        private bool _wasHit;
        public bool Animated => _animated && _wasHit == false;
        
        #if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            var pos = _direction.position;
            var pos2 = pos + _direction.up * 2;
            Gizmos.DrawLine(pos, pos2);
            // Debug.Log("on draw");
        }
        #endif

        public void Push()
        {
            // Debug.Log($"{gameObject.name} pushed");
            _wasHit = true;
            _rb.isKinematic = false;
            _rb.AddTorque(_direction.right * _torque, ForceMode.VelocityChange);
            _rb.AddForce(_direction.up * _force, ForceMode.VelocityChange);   
        }
    }
}