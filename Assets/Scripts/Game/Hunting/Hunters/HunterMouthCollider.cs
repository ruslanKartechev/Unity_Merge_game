using System;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterMouthCollider : MonoBehaviour
    {
        [SerializeField] private int _layer;
        [SerializeField] private Collider _collider;

        public Action<Collider> Callback { get; set; }

        
        public void Activate(bool active)
        {
            _collider.enabled = active;
            enabled = active;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_collider == null)
                _collider = GetComponent<Collider>();
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _layer)
                return;
            Callback?.Invoke(other);
        }
        
    }
}