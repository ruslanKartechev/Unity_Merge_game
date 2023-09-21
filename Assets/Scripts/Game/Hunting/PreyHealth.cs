using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour, IPreyHealth
    {
        public event Action OnDead;
        [SerializeField] private Transform _biteBone;
        [SerializeField] private PreyHealthDisplay _display;
        [SerializeField] private ParticleSystem _particles;
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
            _display.SetHealth(1);
            _display.Hide();
            _effect = GetComponent<IPreyDamageEffect>();
        }

        public void Show()
        {
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
            var percent = _health / _maxHealth;
            Debug.Log($"MaxHealth: {_maxHealth}, damage: {args.Damage}, percent: {percent}");
            if(_display.isActiveAndEnabled == false)
                _display.Show();
            _display.RemoveHealth(percent);
            _particles.transform.position = args.Position;
            _particles.Play();
            if (percent <= 0)
            {
                _isDamageable = false;
                _effect.PlayDead();
                OnDead?.Invoke();
                return;
            }
            _effect.PlayDamaged();
            _animator.Injured(percent);
        }

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