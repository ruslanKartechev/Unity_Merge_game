using Game.Merging;
using TMPro;
using UnityEngine;

namespace Game.UI.Merging
{
    public class SuperEggUI : MonoBehaviour, ISuperEggUI
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _labelText;
        [SerializeField] private GameObject _block;
        private SuperEgg _egg;
        
        public void Show(SuperEgg egg)
        {
            _egg = egg;
            _timerText.text = egg.TimeLeftString;
            _labelText.text = egg.Label;
            _block.SetActive(true);
        }

        public void Hide()
        {
            _block.SetActive(false);
        }

        public void ShowLabel()
        {
            _labelText.enabled = true;
        }

        public void HideLabel()
        {
            _labelText.enabled = false;
        }
    }

    public interface ISuperEggUI
    {
        void Show(SuperEgg egg);
        void Hide();
        void ShowLabel();
        void HideLabel();
    }
}