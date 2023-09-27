using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour, IPreyHealth
    {
        public event Action OnDead;
        [SerializeField] private bool _canBite = true;
        [SerializeField] private Transform _biteBone;
        [SerializeField] private PreyHealthDisplay _display;
        [SerializeField] private List<Transform> _points;
        [Space(10)]
        [SerializeField] private PreyAnimator _animator;

        private float _maxHealth;
        private float _health;
        private bool _isDamageable;
        private IPreyDamageEffect _effect;
        
        public void Init(float maxHealth)
        {
            _isDamageable = true;
            _health = _maxHealth = maxHealth;
            _display.InitMaxHealth(maxHealth);
            _display.Hide();
            _effect = GetComponent<IPreyDamageEffect>();
        }

        public void Show()
        {
            // Debug.Log($"SHOW Health display: {showCount}");
            _display.Show();
        }

        public void Hide()
        {
            _display.Hide();
        }

        public void Damage(DamageArgs args)
        {
            if (!_isDamageable)
                return;
            _health -= args.Damage;
            if (_health < 0)
                _health = 0;
            _display.RemoveHealth(_health);
            if (_health <= 0)
            {
                _isDamageable = false;
                OnDead?.Invoke();
                return;
            }
            _effect.PlayAt(args.Position);
            _effect.PlayDamaged();
            _animator.Injured(_health / _maxHealth);
        }

        public bool CanBite() => _canBite;
        
        public Transform GetBiteParent() => _biteBone;
        
        public Transform GetClosestBitePosition(Vector3 point)
        {
            var closestD2 = float.MaxValue;
            var result = _points[0];
            foreach (var tr in _points)
            {
                var d2 = (tr.position - point).sqrMagnitude;
                if (d2 < closestD2)
                {
                    closestD2 = d2;
                    result = tr;
                }
            }
            return result;
        }
    }
}