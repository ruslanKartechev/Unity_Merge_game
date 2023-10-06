using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class OptionSection : MonoBehaviour
    {
        [SerializeField] private Button _leftBtn;
        [SerializeField] private Button _rightBtn;
        [SerializeField] private TextMeshProUGUI _text;
        
        
        public void Init(UnityAction leftBtn, UnityAction rightBtn)
        {
            _leftBtn.onClick.AddListener(leftBtn);    
            _rightBtn.onClick.AddListener(rightBtn);
        }

        public void Output(bool val) => _text.text = val.ToString();
        
        public void Output(int val) => _text.text = val.ToString();
        
        public void OutputMoney(float val) => _text.text = $"{val}$";
        
    }
}