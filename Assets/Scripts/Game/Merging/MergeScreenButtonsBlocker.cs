using UnityEngine;
using UnityEngine.UI;

namespace Game.Merging
{
    public class MergeScreenButtonsBlocker : MonoBehaviour
    {
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _mergeBtn;
        [SerializeField] private Button _shopBtn;
        [SerializeField] private Button _closeShopBtn;

        public void OnlyShop()
        {
            _shopBtn.interactable = true;
            _playBtn.interactable =false;
            _mergeBtn.interactable = false;
            _closeShopBtn.interactable = false;
        }

        public void OnlyExitShop()
        {
            _shopBtn.interactable = false;
            _playBtn.interactable = false;
            _mergeBtn.interactable = false;
            _closeShopBtn.interactable = true;
        }

        public void All()
        {
            _shopBtn.interactable = true;
            _playBtn.interactable = true;
            _mergeBtn.interactable = true;
            _closeShopBtn.interactable = true;
        }
        
        
        
        public void None()
        {
            _shopBtn.interactable = false;
            _playBtn.interactable = false;
            _mergeBtn.interactable = false;
            _closeShopBtn.interactable = false;
        }
    }
}