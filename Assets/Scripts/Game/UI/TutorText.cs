using Common;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class TutorText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ScalePulser _pulser;
        
        public void Show(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            _pulser.Begin();
        }

        public void Hide()
        {
            _pulser.Stop();
            gameObject.SetActive(false);
        }
    }
}