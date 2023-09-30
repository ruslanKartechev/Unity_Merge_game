using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.UI.Map
{
    public class CurrentMapLevelPointer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        private Sequence _sequence;
        
        public void ShowAt(Vector3 position, int level)
        {
            _levelText.text = $"{level}";
            transform.position = position;            
            gameObject.SetActive(true);
            Scale();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Scale()
        {
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
                return;
            #endif
            transform.localScale = Vector3.one * .95f;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(Vector3.one * 1.15f, .6f).SetEase(Ease.Linear));
            _sequence.Append(transform.DOScale(Vector3.one * .95f, .6f).SetEase(Ease.Linear));
            _sequence.SetLoops(-1);
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}