using Common.Ragdoll;
using Game.Hunting;
using UnityEngine;
using System.Collections;

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
        [Space(10)]
        [SerializeField] private float _moneyFlyDelay;
        [SerializeField] private Transform _moneyPoint;
        [SerializeField] private bool _useFlyMoney = true;
        [SerializeField] private float _moneyReward = 100f;

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