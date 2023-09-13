using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassUIButton : MonoBehaviour
    {
        public event Action<MergeClassUIButton> onClicked; 
        [SerializeField] private Button _button;
        [SerializeField] private Image _highlightImage;

        private void Start()
        {
            _button.onClick.AddListener(OnClicked);
        }
        
        public void Activate()
        {
            _highlightImage.enabled = true;
        }

        public void Deactivate()
        {
            _highlightImage.enabled = false;
        }
        
        private void OnClicked()
        {
            onClicked?.Invoke(this);
        }
    }
}