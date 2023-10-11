using System;
using System.Collections.Generic;
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
        [SerializeField] private List<GameObject> _environment;
        
        public void Shop()
        {
            _shop.enabled = true;
            _main.gameObject.SetActive(false);
            _shop.gameObject.SetActive(true);
            foreach (var go in _environment)
                go.SetActive(false);
            OnShop?.Invoke();
        }

        public void Main()
        {
            _stats.enabled = true;
            _main.enabled = true;
            _shop.gameObject.SetActive(false);
            _main.gameObject.SetActive(true);
            foreach (var go in _environment)
                go.SetActive(true);
            OnMain?.Invoke();
        }
    }
}