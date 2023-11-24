using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Dev
{
    public class TestLoader : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _text;

        public void WaitAndCallback(float time, Action onEnd)
        {
            StopAllCoroutines();
            StartCoroutine(Working(time, onEnd));
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private IEnumerator Working(float time, Action callback)
        {
            var left =   time;
            while (left > 0)
            {
                left -= Time.deltaTime;
                _text.text = $"Time left: {left:N1}";
                yield return null;
            }
            callback.Invoke();
        }
        
    }
}