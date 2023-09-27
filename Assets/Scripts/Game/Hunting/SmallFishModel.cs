using UnityEngine;

namespace Game.Hunting
{
    public class SmallFishModel : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;

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