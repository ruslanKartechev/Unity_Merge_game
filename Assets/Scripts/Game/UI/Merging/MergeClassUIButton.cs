using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassUIButton : MonoBehaviour
    {
        public event Action<MergeClassUIButton> onClicked; 
        [SerializeField] private Button _button;
        [SerializeField] private Image _coloredImage;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _chosenColor;

        private void Start()
        {
            _button.onClick.AddListener(OnClicked);
        }
        
        public void Activate()
        {
            _coloredImage.color = _chosenColor;
        }

        public void Deactivate()
        {
            _coloredImage.color = _defaultColor;
        }
        
        private void OnClicked()
        {
            onClicked?.Invoke(this);
        }
    }
}