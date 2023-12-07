using UnityEngine;

namespace Creatives.Kong
{
    public class KongTrigger : MonoBehaviour
    {
        [SerializeField] private string _attackKey;
        [SerializeField] private Animator _animator;
        
        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IKongPushTarget>(out var target))
            {
                if(target.Animated)
                    _animator.Play(_attackKey);
                target.Push();
            }
        }
    }
}