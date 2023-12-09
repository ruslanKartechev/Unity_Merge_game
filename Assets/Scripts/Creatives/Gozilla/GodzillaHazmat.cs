using Common.Ragdoll;
using Game.Hunting;
using UnityEngine;

namespace Creatives.Gozilla
{
    public class GodzillaHazmat : MonoBehaviour
    {
        [SerializeField] private Transform _jumpTo;
        [SerializeField] private Animator _animator;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private ColdWeapon _weapon;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Rigidbody _bone;

        public Transform JumpTo => _jumpTo;
        
        public void Grab(Joint mouth)
        {
            if (_particles != null)
            {
                _particles.gameObject.SetActive(true);
                _particles.Play();
            }
            _animator.enabled = false;
            _ragdoll.Activate();
            _weapon?.Drop();
            mouth.connectedBody = _bone;
        }
        
    }
}