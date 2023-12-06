using Common.Ragdoll;
using Game.Hunting;
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
        [SerializeField] private GameObject _weapon;

        private void Start()
        {
            _animator.SetFloat("Offset", UnityEngine.Random.Range(0f, 1f));
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
            _ragdoll.ActivateAndPush(vec.normalized * _pushForce);
            
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

            if (_weapon != null)
            {
                if (_weapon.TryGetComponent<ColdWeapon>(out var weapon))
                {
                    weapon.Drop();
                }
            }
        }
    }
}