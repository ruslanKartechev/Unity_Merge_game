using Common.UILayout;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeGridUI : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private LayoutSwitcher _layoutSwitcher;
        [SerializeField] private MergeInputUI _mergeInput;
        
        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButton);
        }

        private void OnCloseButton()
        {
            _closeButton.interactable = false;
            _storeButton.interactable = false;
            _mergeInput.Deactivate();
            _layoutSwitcher.SetLayout(0, () =>
            {
                _storeButton.interactable = true;
            });   
        }

        public void Activate()
        {
            _layoutSwitcher.SetLayout(1, () =>
            {
                _closeButton.interactable = true;
                _storeButton.interactable = true;
                _mergeInput.Activate();
            });
        }
    }
}