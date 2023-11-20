﻿using TMPro;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.UI.Elements
{
    [DefaultExecutionOrder(0)]
    public class MoneyUI : MonoBehaviour, IMoneyUI 
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private PunchAnimator _punchAnimator;
        [SerializeField] private RectTransform _flyTo;

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

        public Vector3 GetFlyToPosition()
        {
            return _flyTo.position;
        }
    }
}