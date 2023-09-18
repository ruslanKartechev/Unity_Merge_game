﻿using Common.UILayout;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeGridUI : MonoBehaviour
    {
        [SerializeField] private Button _storeButton;
        [SerializeField] private MergeInputUI _inputUI;
        
        private void OnCloseButton()
        {
            _storeButton.interactable = false;
            _inputUI.Deactivate();
        }

        public void Activate()
        {
            _storeButton.interactable = true;
            _inputUI.Activate();
        }
    }
}