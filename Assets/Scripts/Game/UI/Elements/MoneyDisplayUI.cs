using System;
using TMPro;
using UnityEngine;

namespace Game.UI.Elements
{
    public class MoneyDisplayUI : MonoBehaviour, IMoneyDisplay 
    {
        private const float Punch = 0.02f;
        private const float PunchTime = 0.25f;
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private PunchAnimator _punchAnimator;

        public void UpdateCount(bool animated = true)
        {
            _text.text = $"{Container.PlayerData.Money}";
            if (!animated)
                return;
            _punchAnimator.PunchAnimate();
        }

   
        public void Highlight()
        {
            Debug.Log("highliting");
        }
    }
}