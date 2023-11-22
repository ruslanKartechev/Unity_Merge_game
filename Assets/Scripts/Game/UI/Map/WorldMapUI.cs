using System.Collections;
using Common.UIPop;
using Game.UI.Hunting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Map
{
    public class WorldMapUI : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MapLevelsDisplay _levelsDisplay;
        [SerializeField] private PopAnimator _popAnimator;
        [SerializeField] private PowerDisplay _powerDisplay;
        [SerializeField] private Button _playButton;

        public PowerDisplay Power => _powerDisplay;
        public MapLevelsDisplay LevelsDisplay => _levelsDisplay;
        public Button PlayButton => _playButton;
        
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

        public void LevelsOnly()
        {
            _levelsDisplay.gameObject.SetActive(true);
            _popAnimator.HideAll();
        }
        
        public void PopAll()
        {
            _popAnimator.HideAndPlay();
        }
    }
}