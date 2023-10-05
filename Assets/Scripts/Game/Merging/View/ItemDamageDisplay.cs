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
        }

    }
}