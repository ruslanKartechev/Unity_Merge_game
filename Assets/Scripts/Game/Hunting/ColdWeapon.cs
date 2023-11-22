using UnityEngine;

namespace Game.Hunting
{
    public class ColdWeapon : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _coll;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Drop()
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _coll.enabled = true;
        }
        
        
        public void Drop(Vector3 force)
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _coll.enabled = true;
            _rb.AddForce(force, ForceMode.Impulse);
        }

    }
}