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
            SetLevel(level);
            SetPosition(position);
            gameObject.SetActive(true);
            ScaleAnimation();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;            
        }

        public void SetLevel(int level)
        {
            _levelText.text = $"{level}";
        }

        public void ScaleDown()
        {
            transform.localScale = Vector3.one * .7f;
        }
        
        public void MoveFromTo(Vector3 from, Vector3 to, float duration)
        {
            var tr= transform;
            tr.position = from;            
            gameObject.SetActive(true);
            tr.DOMove(to, duration);
            tr.DOScale(Vector3.one, duration).OnComplete(() =>
            {
                ScaleAnimation();
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ScaleAnimation()
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