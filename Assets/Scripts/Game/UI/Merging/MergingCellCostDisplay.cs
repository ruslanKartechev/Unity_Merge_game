using TMPro;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergingCellCostDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _costText;

        public void SetCost(float cost)
        {
            _costText.text = $"{cost}";
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