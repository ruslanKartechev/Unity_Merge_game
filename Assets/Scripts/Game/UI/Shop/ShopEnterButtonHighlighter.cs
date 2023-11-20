using Common;
using Game.Core;
using UnityEngine;

namespace Game.UI.Shop
{
    [DefaultExecutionOrder(50)]
    public class ShopEnterButtonHighlighter : MonoBehaviour
    {
        [SerializeField] private float _moneyThreshold = 100;
        [SerializeField] private ScalePulser _scalePulser;
        [SerializeField] private GameObject _redDot;

        public void OnEnable()
        {
            if (GC.PlayerData.Money >= _moneyThreshold)
            {
                _scalePulser.Begin();
                _redDot.SetActive(true);
            }
            else
            {
                _scalePulser.Reset();
                _redDot.SetActive(false); 
            }
        }
        
    }
}