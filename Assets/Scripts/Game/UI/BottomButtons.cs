using System;
using UnityEngine;


namespace Game.UI
{
    public class BottomButtons : MonoBehaviour
    {
        [SerializeField] private SpriteChangeButton _collection;
        [SerializeField] private SpriteChangeButton _main;
        [SerializeField] private SpriteChangeButton _map;

        public Action OnMain { get; set; }
        public Action OnCollection { get; set; }
        public Action OnMap { get; set; }
        
        
        public void SetCollection()
        {
            _collection.SetActive();
            _main.SetUsual();
            _map.SetUsual();
        }
        
        public void SetMain()
        {
            _collection.SetUsual();
            _main.SetActive();
            _map.SetUsual();
        }

        
        public void SetMap()
        {
            _collection.SetUsual();
            _main.SetUsual();
            _map.SetActive();
        }

        private void OnEnable()
        {
            _collection.Btn.onClick.AddListener(OnCollectionBtn);
            _main.Btn.onClick.AddListener(OnMainBtn);
            _map.Btn.onClick.AddListener(OnMapBtn);
        }

        private void OnDisable()
        {
            _collection.Btn.onClick.RemoveListener(OnCollectionBtn);
            _main.Btn.onClick.RemoveListener(OnMainBtn);
            _map.Btn.onClick.RemoveListener(OnMapBtn);   
        }

        private void OnCollectionBtn()
        {
            _collection.Scale();
            OnCollection?.Invoke();      
        }
        
        private void OnMainBtn()
        {
            _main.Scale();
            OnMain?.Invoke();
        }
        
        private void OnMapBtn()
        {
            _map.Scale();
            OnMap?.Invoke();   
        }


    }
}