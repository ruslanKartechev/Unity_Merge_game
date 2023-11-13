using UnityEngine;

namespace Game.Hunting
{
    public class PreyHealth : MonoBehaviour
    {
        [SerializeField] private bool _showHealthFromStart = true;
        [SerializeField] private PreyHealthDisplay _display;
        [SerializeField] private PreyAnimator _animator;

        private float _maxHealth;
        private float _health;
        private bool _isDamageable;
        private IPreyDamageEffect _effect;
        private bool _shownHealth;
        private bool _isGrabbed;
        
        public void Init(float maxHealth)
        {
            _isDamageable = true;
            _health = _maxHealth = maxHealth;
            _display.InitMaxHealth(maxHealth);
            _effect = GetComponent<IPreyDamageEffect>();
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
            _display.OnHealthChange(_health, _maxHealth);
            _effect.Particles(args.Position);
            if (_health <= 0)
            {
                _isDamageable = false;
                return;
            }
            // _effect.Damaged();
            _animator.Injured(_health / _maxHealth);
        }


        public bool IsAlive() => _health > 0;
    }

}