using TMPro;
using UnityEngine;

namespace Common
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField, Range(0f,1f)] private float _updatePeriod = 1f;

        private float _elapsed = 0f;
        private int _frames;
        
        public void Begin()
        {
            this.enabled = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed >= _updatePeriod)
            {
                _text.text = $"{(int)(1f / Time.unscaledDeltaTime)}";
                _elapsed = 0f;
            }
        }
    }
}