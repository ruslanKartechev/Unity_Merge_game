using Common;
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
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;
        [SerializeField] private MergeGridUI _mergeGrid;
        [SerializeField] private MergeInputUI _mergeInputUI;
        [Space(10)] 
        [SerializeField] private ShopUI _shopUI;
        [SerializeField] private MergeCanvasSwitcher _canvasSwitcher;
        [SerializeField] private Button _shopButton;
        
        public void Show()
        {
            _canvasSwitcher.Main();
            UpdateLevel();
            UIC.UpdateMoneyAndCrystals();
            ShowMergeGrid();
            _mergeManager.MergeInput.Activate();
        }

        private void ShowShop()
        {
            _mergeInputUI.Deactivate();
            _canvasSwitcher.Shop();     
            _shopUI.Show(Show);
        }

        public void UpdateLevel()
        {
            _levelDisplay.SetLevel(GC.PlayerData.LevelTotal + 1);
        }
        
        
        private void Start()
        {
            _mergeManager.Init();
            var input = _mergeManager.MergeInput;
            input.SetUI(_mergeInputUI);
            _mergeInputUI.SetInput(input);
            _shopButton.onClick.AddListener(ShowShop);
            _playButton.onClick.AddListener(_mergeManager.MoveToPlayLevel);
            LoadingCurtain.Open(() => {});
            Show();
        }

        private void ShowMergeGrid()
        {
            _classesSwitcher.ShowDefault();
            _mergeGrid.Activate();
        }

    }
}