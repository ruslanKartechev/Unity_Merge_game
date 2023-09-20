using System;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour, IPreyHealth
    {
        public event Action OnDead;
        [SerializeField] private Transform _biteBone;
        [SerializeField] private PreyHealthDisplay _display;
        [SerializeField] private ParticleSystem _particles;
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
            if(_display.isActiveAndEnabled == false)
                _display.Show();
            _display.RemoveHealth(percent);
            _particles.transform.position = args.Position;
            _particles.Play();
            _effect.Play();
            if (percent <= 0)
            {
                _isDamageable = false;
                OnDead?.Invoke();
            }   
        }

        public Transform GetBiteBone() => _biteBone;
    }
}