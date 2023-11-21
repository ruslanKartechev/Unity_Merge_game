using UnityEngine;
using UnityEngine.UI;

namespace Game.Merging
{
    public class TutorButtonsBlocker : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _shopCloseButton;
        [SerializeField] private ScrollRect _scroll;
            
        public void BlockForMerge(bool block)
        {
            _playButton.interactable = !block;
            _shopButton.interactable = !block;
        }

        public void BlockToEnterShop()
        {
            _playButton.interactable = false;
            _shopButton.interactable = true;
        }

        public void BlockToBuyInShop()
        {
            _shopCloseButton.interactable = false;
            _scroll.vertical = false;
        }

        public void BlockToExitInShop()
        {
            _shopCloseButton.interactable = true;
            _scroll.vertical = false;
        }
        
        public void EnableAll()
        {
            _shopButton.interactable = true;
            _playButton.interactable = true;
            _shopCloseButton.interactable = true;
            _scroll.vertical = true;
        }
    }
}