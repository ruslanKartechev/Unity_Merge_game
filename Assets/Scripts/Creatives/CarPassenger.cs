using UnityEngine;

namespace Creatives
{
    public class CarPassenger : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Start()
        {
            _animator.Play("Help");
        }
    }
}