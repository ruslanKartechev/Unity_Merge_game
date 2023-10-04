using System;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeCanvasSwitcher : MonoBehaviour
    {
        public event Action OnShop;
        public event Action OnMain;
        
        [SerializeField] private Canvas _shop;
        [SerializeField] private Canvas _main;
        [SerializeField] private Canvas _stats;
        
        public void Shop()
        {
            Debug.Log("[Switcher] SHOP");
            _stats.enabled = true;
            _main.enabled = false;
            _shop.enabled = true;
            _shop.gameObject.SetActive(true);
            OnShop?.Invoke();
        }

        public void Main()
        {
            Debug.Log("[Switcher] MAIN");
            _stats.enabled = true;
            _main.enabled = true;
            _shop.gameObject.SetActive(false);
            _main.gameObject.SetActive(true);
            OnMain?.Invoke();
        }
    }
}