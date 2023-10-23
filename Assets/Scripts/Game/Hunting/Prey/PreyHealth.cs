using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour, IPreyHealth, IFishTarget, IAirTarget
    {
        [SerializeField] private bool _showHealthFromStart = true;
        [SerializeField] private bool _canBite = true;
        [SerializeField] private PreyHealthDisplay _display;
        [Space(10)] 
        [SerializeField] private Transform _airTarget;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [SerializeField] private PreyAnimator _animator;
        private HashSet<IHealthListener> _listeners = new HashSet<IHealthListener>();

        private float _maxHealth;
        private float _health;
        private bool _isDamageable;
        private IPreyDamageEffect _effect;
        private bool _shownHealth;
        private bool _isGrabbed;
        
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

            if (_isGrabbed)
            {
                DamageInAir(args);
                return;
            }
                        
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

        private void DamageInAir(DamageArgs args)
        {
            _health -= args.Damage;
            if (_health < 0)
                _health = 0;
            _display.OnHealthChange(_health, _maxHealth);
            Debug.Log($"Damaging while in air");
        }

        public bool IsAlive() => _health > 0;
        
        public Transform GetFlyToTransform() => _airTarget;
        
        public Transform MoverParent() => transform.parent;

        public void GrabTo(Transform transform)
        {
            _isGrabbed = true;
            if (_positionAdjuster != null)
                _positionAdjuster.enabled = false;
            transform.parent = transform;
        }

        public void DropAlive()
        {
            transform.SetParent(null);
            if (_positionAdjuster != null)
                _positionAdjuster.enabled = true;   
        }

        public void DropDead()
        {
            transform.SetParent(null);
            foreach (var listener in _listeners)
                listener.OnHealthChange(_health, _maxHealth);
        }
        
        // MAKE IT APPROPRIATE !!!!!!!!
        public Vector3 GetShootAtPosition()
        {
            return transform.position + Vector3.one;
        }
        
        public bool CanBindTo() => _canBite;
        
    }

}