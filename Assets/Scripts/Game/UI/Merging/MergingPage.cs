using Common;
using Common.UILayout;
using Game.Merging;
using Game.UI.Elements;
using Game.UI.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergingPage : MonoBehaviour, IMergingPage
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private Button _playButton;
        [SerializeField] private MoneyDisplayUI _moneyDisplay;
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;
        [SerializeField] private LayoutSwitcher _layoutSwitcher;
        [SerializeField] private Button _gridButton;
        [SerializeField] private MergeGridUI _mergeGrid;
        [SerializeField] private MergeInputUI _mergeInputUI;
        [Space(10)] 
        [SerializeField] private ShopUI _shopUI;
        [SerializeField] private MergeCanvasSwitcher _canvasSwitcher;
        [SerializeField] private Button _shopButton;
        

        public void Show()
        {
            _canvasSwitcher.Main();
            _layoutSwitcher.SetLayout(0, () => {}, false);
            UpdateMoney();
            UpdateCrystals();
            UpdateLevel();
            _mergeManager.MergeInput.Activate();
        }

        private void ShowShop()
        {
            _mergeInputUI.Deactivate();
            _canvasSwitcher.Shop();     
            _shopUI.Init();
        }

        public void UpdateMoney()
        {
            _moneyDisplay.UpdateCount();
        }

        public void UpdateCrystals()
        { }

        public void UpdateLevel()
        {
            _levelDisplay.SetLevel(GC.PlayerData.LevelTotal + 1);
        }
        
        
        private void Start()
        {
            _mergeManager.Init();
            var input = _mergeManager.MergeInput;
            input.SetUI(this, _mergeInputUI);
            _mergeInputUI.SetInput(input);
            
            _gridButton.onClick.AddListener(OnGridButton);
            _shopButton.onClick.AddListener(ShowShop);
            _playButton.onClick.AddListener(_mergeManager.MoveToPlayLevel);
            LoadingCurtain.Open(() => {});
            Show();
        }

        private void OnGridButton()
        {
            _classesSwitcher.ShowDefault();
            _mergeGrid.Activate();
        }

    }
}