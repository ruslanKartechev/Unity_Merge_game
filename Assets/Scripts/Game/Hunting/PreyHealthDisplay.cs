using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting
{
    public class PreyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _fillImageBack;
        [SerializeField] private float _changeTime = 0.5f;
        [SerializeField] private float _changeDelay = 0.5f;
        [SerializeField] private TextMeshProUGUI _text;
        private Coroutine _changing;
        private float _currentHealth;
        private float _maxHealth;
        
        
        public void Show(bool animated = true)
        {
            gameObject.SetActive(true);
            if (animated)
            {
                var tr = transform;
                tr.localScale = Vector3.zero;
                tr.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBounce);
            }
            if (_currentHealth == _maxHealth)
                return;
            SetHealth(_currentHealth);
            _changing = StartCoroutine(Changing(_maxHealth));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void InitMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            SetHealth(maxHealth);
        }
        
        public void SetHealth(float health)
        {
            _currentHealth = health;
            var percent = _currentHealth / _maxHealth;
            _fillImage.fillAmount = _fillImageBack.fillAmount = percent;
            SetText(health);
        }

        public void RemoveHealth(float healthLeft)
        {
            var previousHealth = _currentHealth;
            _currentHealth = healthLeft;
            if (!isActiveAndEnabled)
                return;
            StopChange();
            SetText(_currentHealth);
            _changing = StartCoroutine(Changing(previousHealth));
        }

        private void StopChange()
        {
            if(_changing != null)
                StopCoroutine(_changing);
        }

        private IEnumerator Changing(float healthFrom)
        {
            Debug.Log($"Chaning health to: {_currentHealth}, max: {_maxHealth}");
            var targetPercent = _currentHealth / _maxHealth;
            var start = _fillImageBack.fillAmount;
            _fillImage.fillAmount = targetPercent;
            yield return new WaitForSeconds(_changeDelay);
            var elapsed = 0f;
            while (elapsed < _changeTime)
            {
                var percent = Mathf.Lerp(start, targetPercent, elapsed / _changeTime);
                _fillImageBack.fillAmount = percent;
                // SetText(percent);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _fillImageBack.fillAmount = targetPercent;
            _fillImageBack.fillAmount = targetPercent;
        }

        private void SetText(float health)
        {
            _text.text = $"{Mathf.RoundToInt(health)}";
        }
    }
}