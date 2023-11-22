using Common.UIEffects;
using Game.Core;
using Game.Merging;
using Game.Merging.Interfaces;
using Game.UI.Elements;
using Game.UI.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    [DefaultExecutionOrder(50)]
    public class MergeUIPage : MonoBehaviour, IMergingPage
    {
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _mergeAllBtn;
        [SerializeField] private ScaleEffect _mergeBtnScale;
        [Space(10)]
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;
        [SerializeField] private MergeGridUI _mergeGrid;
        [SerializeField] private MergeInputUI _mergeInputUI;
        [Space(10)] 
        [SerializeField] private ShopUI _shopUI;
        [SerializeField] private MergeCanvasSwitcher _canvasSwitcher;
        [SerializeField] private Button _shopButton;
        private IMergeManager _mergeManager;
        private IMergeInput _mergeInput;
        

        public void Init(IMergeManager mergeManager, IMergeInput input)
        {
            _mergeManager = mergeManager;
            _mergeInput = input;
            input.SetStash(_mergeInputUI);
            _mergeInputUI.SetInput(input);
            _shopButton.onClick.AddListener(ShowShop);
            _playBtn.onClick.AddListener(_mergeManager.MoveToPlayLevel);
            _mergeAllBtn.onClick.AddListener(MergeAll);
            // LoadingCurtain.Open(() => {});
            Show();    
        }
        
        public void Show()
        {
            _canvasSwitcher.Main();
            UIC.Money.UpdateCount(false);
            _levelDisplay.SetCurrent();
            ShowMergeGrid();
            _mergeInput.Activate();
            GC.Input.Enable();
        }

        private void ShowShop()
        {
            GC.Input.Disable();
            _mergeInputUI.Deactivate();
            _canvasSwitcher.Shop();     
            _shopUI.Show(Show);
        }
        
        private void ShowMergeGrid()
        {
            _classesSwitcher.Init();
            _classesSwitcher.ShowFirstWithItemsOrDefault();
            // _classesSwitcher.ShowDefault();
            _mergeGrid.Activate();
        }
        
        private void MergeAll()
        {
            _mergeBtnScale.Play();
            _mergeManager.MergeAllInStash();
            _classesSwitcher.UpdateCurrent();
        }
        
    }
}