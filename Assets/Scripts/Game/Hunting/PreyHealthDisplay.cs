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
        private float _currentTarget;
        private Coroutine _changing;
        
        public void Show(bool animated = true)
        {
            gameObject.SetActive(true);
            if (animated)
            {
                var tr = transform;
                tr.localScale = Vector3.zero;
                tr.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBounce);
            }
            _changing = StartCoroutine(Changing());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetHealth(float percent)
        {
            _fillImage.fillAmount = _fillImageBack.fillAmount = percent;
            _currentTarget = percent;
            SetText(percent);
        }

        public void RemoveHealth(float healthLeft)
        {
            _currentTarget = healthLeft;
            if (!isActiveAndEnabled)
                return;
            StopChange();
            _changing = StartCoroutine(Changing());
        }

        private void StopChange()
        {
            if(_changing != null)
                StopCoroutine(_changing);
        }

        private IEnumerator Changing()
        {
            _fillImage.fillAmount = _currentTarget;
            yield return new WaitForSeconds(_changeDelay);
            var elapsed = 0f;
            var start = _fillImageBack.fillAmount;
            while (elapsed < _changeTime)
            {
                var percent = Mathf.Lerp(start, _currentTarget, elapsed / _changeTime);
                _fillImageBack.fillAmount = percent;
                SetText(percent);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _fillImageBack.fillAmount = _currentTarget;
            _fillImageBack.fillAmount = _currentTarget;
        }

        private void SetText(float percent)
        {
            _text.text = $"{(int)(percent * 100)}";
        }
    }
}