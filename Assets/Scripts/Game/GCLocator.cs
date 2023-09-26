using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Common.SlowMotion;
using Game.Merging;
using Game.Shop;
using UnityEngine;

namespace Game
{
    public class GCLocator : MonoBehaviour, IGlobalContainerLocator
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private IDataSaver _dataSaver;
        [SerializeField] private SceneSwitcher _sceneSwitcher;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private HuntersRepository _hunters;
        [SerializeField] private ActiveGroupSO activeGroupSo;
        [SerializeField] private LevelsRepository _levelsRepository;
        [SerializeField] private MergeItemsStashSO _stashSO;
        [SerializeField] private MergeTable _mergeTable;
        [SerializeField] private MergeItemViews itemViews;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private ShopItemsViews _shopItemsViews;
        [SerializeField] private SlowMotionManager _slowMotionEffect;

        public void InitContainer()
        {
            GC.PlayerData = _playerData;
            GC.SceneSwitcher = _sceneSwitcher;
            GC.DataSaver = _dataSaver;
            GC.LevelManager = _levelManager;
            GC.HuntersRepository = _hunters;
            GC.ActiveGridSO = activeGroupSo;
            GC.LevelRepository = _levelsRepository;
            GC.ItemsStash = _stashSO;
            GC.ItemViews = itemViews;
            GC.MergeTable = _mergeTable;
            GC.ShopItems = _shopItems;
            GC.ShopItemsViews = _shopItemsViews;
            GC.SlowMotion = _slowMotionEffect;
            _hunters.Init();
            _shopItemsViews.Init();
        }
    }
}