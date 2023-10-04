using Common.UIEffects;
using Game.Merging;
using Game.UI.Elements;
using Game.UI.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    [DefaultExecutionOrder(50)]
    public class MergingPage : MonoBehaviour, IMergingPage
    {
        [SerializeField] private MergeManager _mergeManager;
        [Space(10)]
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
        
        
        private void Start()
        {
            if (GC.PlayerData == null)
            {
                Debug.Log($"Container references not found! Game Should Start From ''Start'' SCENE ");
                SceneManager.LoadScene("Start");
                return;
            }
            _mergeManager.Init();
            var input = _mergeManager.MergeInput;
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
            UIC.UpdateMoneyAndCrystals();
            UpdateLevel();
            ShowMergeGrid();
            _mergeManager.MergeInput.Activate();
            GC.Input.Enable();
        }

        private void ShowShop()
        {
            GC.Input.Disable();
            _mergeInputUI.Deactivate();
            _canvasSwitcher.Shop();     
            _shopUI.Show(Show);
        }

        public void UpdateLevel()
        {
            _levelDisplay.SetLevel(GC.PlayerData.LevelTotal + 1);
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