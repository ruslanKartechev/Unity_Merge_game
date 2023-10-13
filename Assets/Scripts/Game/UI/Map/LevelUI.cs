using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Map
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private Image _greenIcon;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelTextPassed;
        [Space(10)]
        [SerializeField] private Transform _pointerPoint;
        [Space(10)]
        [SerializeField] private GameObject _activeBlock;
        [SerializeField] private GameObject _passiveBlock;

        public Vector3 PointerPosition => _pointerPoint.position;
        
        public void FadeIn(float time)
        {
            SetPassed();
            _greenIcon.color = Color.red;
            _greenIcon.transform.localScale = Vector3.one * 1.22f;
            _greenIcon.transform.DOScale(Vector3.one, time);
            // _greenIcon.DOFade(1f, time);
            _greenIcon.DOColor(Color.white, time);
        }

        private void SetAlpha(float val)
        {
            var color = _greenIcon.color;
            color.a = val;
            _greenIcon.color = color;
        }
        
        public void SetLevel(int level)
        {
            _level = level;
            _levelText.text = $"{_level}";
            _levelTextPassed.text = _levelText.text;
        }

        public void SetPassed()
        {
            _activeBlock.SetActive(true);
            _passiveBlock.SetActive(false);
        }

        public void SetLocked()
        {
            _activeBlock.SetActive(false);
            _passiveBlock.SetActive(true);
        }

        public void HideAll()
        {
            _activeBlock.SetActive(false);
            _passiveBlock.SetActive(false);
        }
    }
}