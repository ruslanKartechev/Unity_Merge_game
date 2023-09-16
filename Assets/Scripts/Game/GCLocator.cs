using Common.Levels;
using Common.Saving;
using Common.Scenes;
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
        [SerializeField] private HuntersRepository huntersRepository;
        [SerializeField] private ActiveGroupSO activeGroupSo;
        [SerializeField] private LevelsRepository _levelsRepository;
        [SerializeField] private MergeItemsStashSO _stashSO;
        [SerializeField] private MergeTable _mergeTable;
        [SerializeField] private MergeItemViewRepository _itemViewRepository;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private ShopItemsViews _shopItemsViews;

        public void InitContainer()
        {
            GC.PlayerData = _playerData;
            GC.SceneSwitcher = _sceneSwitcher;
            GC.DataSaver = _dataSaver;
            GC.LevelManager = _levelManager;
            GC.HuntersRepository = huntersRepository;
            GC.ActiveGridSO = activeGroupSo;
            GC.LevelRepository = _levelsRepository;
            GC.ItemsStash = _stashSO;
            GC.ItemViews = _itemViewRepository;
            GC.MergeTable = _mergeTable;
            GC.ShopItems = _shopItems;
            GC.ShopItemsViews = _shopItemsViews;
            _shopItemsViews.Init();
        }
    }
}