using Game.UI.Shop;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI.StartScreen
{
    public interface IStartPageListener
    {
        void OnPlay();
    }
    
    
    [DefaultExecutionOrder(10)]
    public class StartPage : MonoBehaviour
    {
        [SerializeField] private ShopUI _shop;
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Canvas _shopCanvas;
        [Space(10)]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _collectionButton;
        [SerializeField] private Button _settingsButton;
        private IStartPageListener _listener;
        
        public void InitPage(IStartPageListener listener)
        {
            _listener = listener;
            SubButtons();
            UIC.UpdateMoneyAndCrystals();
        }
        
        private void OpenCollection()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Collection", "w");
        }

        private void OpenSettings()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Settings", "w");
        }

        private void OpenShop()
        {
            CLog.LogWHeader(nameof(StartPage), "Open Shop", "w");
            _mainCanvas.enabled = false;
            _shopCanvas.enabled = true;
            _shop.Show(Show);
        }

        private void Play()
        {
            CLog.LogWHeader(nameof(StartPage), "Play", "w");
            _listener.OnPlay();
        }

        private void Show()
        {
            UIC.UpdateMoneyAndCrystals();
            _mainCanvas.enabled = true;
            _shopCanvas.enabled = false;
        }
        
        private void SubButtons()
        {
            _playButton.onClick.AddListener(Play);
            _shopButton.onClick.AddListener(OpenShop);
            _collectionButton.onClick.AddListener(OpenCollection);
            _settingsButton.onClick.AddListener(OpenSettings);
        }

    }
}