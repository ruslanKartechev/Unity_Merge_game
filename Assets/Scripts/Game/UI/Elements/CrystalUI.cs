using TMPro;
using UnityEngine;

namespace Game.UI.Elements
{
    public class CrystalUI : MonoBehaviour, IMoneyUI 
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private PunchAnimator _punchAnimator;
        
        
        private void Awake()
        {
            UIC.Crystals = this;
        }

        public void UpdateCount(bool animated = true)
        {
            _text.text = $"{GC.PlayerData.Crystal}";
            if (!animated)
                return;
            _punchAnimator.PunchAnimate();
        }

        public void Highlight()
        {
            Debug.Log("Highlight money UI");
        }
    }
}