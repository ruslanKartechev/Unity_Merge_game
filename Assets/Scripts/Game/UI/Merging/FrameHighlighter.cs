using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class FrameHighlighter : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _highlightColor;
        [SerializeField] private float _time;
        private Coroutine _changing;

        public void SetNormal()
        {
            _image.color = _normalColor;
        }

        public void Highlight()
        {
            StopChanging();
            _image.color = _highlightColor;
            _changing = StartCoroutine(Coloring(_highlightColor, _normalColor));
        }

        private void StopChanging()
        {
            if(_changing != null)
                StopCoroutine(_changing);
        }

        private IEnumerator Coloring(Color from, Color to)
        {
            var elapsed = 0f;
            while (elapsed <= _time) 
            {
                _image.color = Color.Lerp(from, to, elapsed / _time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _image.color = to;
        }
    }
}