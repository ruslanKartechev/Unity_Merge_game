using System;
using Game.UI.StartScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassUIButton : MonoBehaviour
    {
        public event Action<MergeClassUIButton> onClicked; 
        [SerializeField] private Button _button;
        [SerializeField] private SpriteChangeButton _changeButton;
        [SerializeField] private TextMeshProUGUI _countText;

        private void Start()
        {
            _button.onClick.AddListener(OnClicked);
        }
        
        public void Activate()
        {
            _changeButton.SetActive();
        }

        public void Deactivate()
        {
            _changeButton.SetUsual();
        }

        public void SetCount(int count) => _countText.text = $"{count}";
        
        private void OnClicked()
        {
            _changeButton.Scale();
            onClicked?.Invoke(this);
        }
    }
}