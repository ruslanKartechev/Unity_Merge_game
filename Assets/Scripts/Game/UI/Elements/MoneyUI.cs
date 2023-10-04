using System;
using TMPro;
using UnityEngine;

namespace Game.UI.Elements
{
    [DefaultExecutionOrder(0)]
    public class MoneyUI : MonoBehaviour, IMoneyUI 
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private PunchAnimator _punchAnimator;

        private void Awake()
        {
            UIC.Money = this;
        }

        public void UpdateCount(bool animated = true)
        {
            _text.text = $"{GC.PlayerData.Money}";
            if (!animated)
                return;
            _punchAnimator.PunchAnimate();
        }
        
        public void UpdateCount(float addedSum, bool animated = true)
        {
            _text.text = $"{GC.PlayerData.Money + addedSum}";
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