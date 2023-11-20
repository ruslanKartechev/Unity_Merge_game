using System.Collections;
using UnityEngine;

namespace Game.UI.Map
{
    public class WorldMapUI : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MapLevelsDisplay _levelsDisplay;

        public MapLevelsDisplay LevelsDisplay => _levelsDisplay;
        
        public void FadeIn()
        {
            _canvasGroup.alpha = 0f;
            _canvas.enabled = true;
            StartCoroutine(FadeFromTo(0f, 1f, _fadeInDuration));
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private IEnumerator FadeFromTo(float from, float to, float time)
        {
            var elapsed = 0f;
            var t = 0f;
            while (t < 1)
            {
                var a = Mathf.Lerp(from, to, t);
                _canvasGroup.alpha = a;
                t = elapsed / time;
                elapsed += Time.deltaTime;
                yield return null;
            }
            _canvasGroup.alpha = to;
        }
    }
}