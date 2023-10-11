using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Merging
{
    public class ItemDamageDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
    
        public void SetDamage(float value)
        {
            _text.text = $"{value}";
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            // transform.DOScale(new Vector3(1f, 0f, 1f), 0.4f).OnComplete(() =>
            // {
            //     gameObject.SetActive(false);
            // });
        }

    }
}