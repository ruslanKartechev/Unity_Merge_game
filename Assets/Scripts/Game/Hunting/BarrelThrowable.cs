using UnityEngine;

namespace Game.Hunting
{
    public class BarrelThrowable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _block;
        [SerializeField] private Transform _direction;
        [SerializeField] private float _force;
        
        
        public void Push()
        {
            _block.gameObject.SetActive(true);
            _block.transform.parent = null;
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(_direction.forward * _force, ForceMode.Impulse);
        }
        
    }
}