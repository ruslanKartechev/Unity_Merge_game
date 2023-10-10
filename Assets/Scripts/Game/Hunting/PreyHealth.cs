using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour, IPreyHealth
    {
        [SerializeField] private bool _showHealthFromStart = true;
        [SerializeField] private bool _canBite = true;
        [SerializeField] private Transform _biteBone;
        [SerializeField] private PreyHealthDisplay _display;
        [SerializeField] private List<Transform> _points;
        [Space(10)]
        [SerializeField] private PreyAnimator _animator;
        private HashSet<IHealthListener> _listeners = new HashSet<IHealthListener>();

        private float _maxHealth;
        private float _health;
        private bool _isDamageable;
        private IPreyDamageEffect _effect;
        private bool _shownHealth;
        public void AddListener(IHealthListener listener) => _listeners.Add(listener);
        
        public void Init(float maxHealth)
        {
            _isDamageable = true;
            _health = _maxHealth = maxHealth;
            _display.InitMaxHealth(maxHealth);
            _effect = GetComponent<IPreyDamageEffect>();
            AddListener(_display);
            if (_showHealthFromStart)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            if (_shownHealth)
                return;
            _shownHealth = true;
            _display.Show();
        }

        public void Hide()
        {
            _shownHealth = false;
            _display.Hide();
        }

        public void Damage(DamageArgs args)
        {
            if (!_isDamageable)
                return;
            
            _health -= args.Damage;
            if (_health < 0)
                _health = 0;
            
            foreach (var listener in _listeners)
                listener.OnHealthChange(_health, _maxHealth);
            _effect.Particles(args.Position);
            if (_health <= 0)
            {
                _isDamageable = false;
                return;
            }
            _effect.Damaged();
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