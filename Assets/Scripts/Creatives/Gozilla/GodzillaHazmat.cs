using Common.Ragdoll;
using Game.Hunting;
using UnityEngine;
using System.Collections;

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
        [Space(10)]
        [SerializeField] private float _moneyFlyDelay;
        [SerializeField] private Transform _moneyPoint;
        [SerializeField] private bool _useFlyMoney = true;
        [SerializeField] private float _moneyReward = 100f;
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
            FlyMoney();
        }
     
        
        private void FlyMoney()
        {
            if (_moneyFlyDelay <= 0)
            {
                CallMoney();
                return;
            }

            StartCoroutine(DelayedMoney(_moneyFlyDelay));

        }

        private void CallMoney()
        {
            if (_useFlyMoney && _moneyPoint != null)
            {
                var creos = CreosUI.Instance;
                if (creos == null)
                    return;
                creos.FlyingMoney.FlySingle(_moneyPoint.position, _moneyReward);
            }    
        }
        
        private IEnumerator DelayedMoney(float time)
        {
            yield return new WaitForSeconds(time);
            CallMoney();
        }
    }
}