using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hunting
{
    public class DarkeningUI : MonoBehaviour
    {
        [SerializeField] private float _alpha = .8f;
        [SerializeField] private float _duration;
        [SerializeField] private Image _image;
        private Coroutine _working;
        
        
        public void Show()
        {
            Stop();
            _working = StartCoroutine(Working(0f, _alpha));            
        }

        public void Hide()
        {
            Stop();
            _working = StartCoroutine(Working(_alpha, 0f));
        }

        public void ShowNow()
        {
            Stop();
            _image.enabled = true;
            SetA(_alpha);   
            _image.enabled = true;
        }

        public void HideNow()
        {
            Stop();
            _image.enabled = false;
        }

        private void SetA(float a)
        {
            var color = _image.color;
            color.a = a;
            _image.color = color;
        }

        private void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        public IEnumerator Working(float from, float to)
        {
            _image.enabled = true;
            var elapsed = 0f;
            while (elapsed <= _duration)
            {
                SetA(Mathf.Lerp(from, to, elapsed/ _duration));
                elapsed += Time.deltaTime;
                yield return null;
            }
            SetA(to);
        }
        
    }
}