using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.UI.Hunting
{
    public class PowerDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform _block;
        [SerializeField] private float _showUpDuration;
        [SerializeField] private float _hideDuration;
        [SerializeField] private Ease _ease;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _textUs;
        [SerializeField] private TextMeshProUGUI _textEnemy;

        
        [ContextMenu("Show")]
        public void Show()
        {
            _block.gameObject.SetActive(true);
            _block.DOKill();
            _block.eulerAngles = new Vector3(-90, 0f, 0f);
            _block.DORotate(Vector3.zero, _showUpDuration).SetEase(_ease);
        }

        public void SetPower(float us, float enemy)
        {
            _textUs.text = $"{us}";
            _textEnemy.text = $"{enemy}";
        }

        public void Hide()
        {
            _block.DOKill();
            _block.eulerAngles = new Vector3(0f, 0f, 0f);
            _block.DORotate(new Vector3(-90, 0f, 0f), _hideDuration).OnComplete(() =>
            {
                _block.gameObject.SetActive(false);
            });
        }
    }
}