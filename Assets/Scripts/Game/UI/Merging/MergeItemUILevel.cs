using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeItemUILevel : MonoBehaviour
    {
        private const float PunchTime = .25f;
        private const float PunchScale = .12f;
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _block;

        public void SetLevel(int level)
        {
            _text.text = $"{level}";
        }

        public void Show()
        {
            _block.SetActive(true);
        }

        public void Hide()
        {
            _block.SetActive(false);
        }

        public void PlayScale()
        {
            transform.localScale = Vector3.one;
            transform.DOPunchScale(PunchScale * Vector3.one, PunchTime);
        }
    }
}