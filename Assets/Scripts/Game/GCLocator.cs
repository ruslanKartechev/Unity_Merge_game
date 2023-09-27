﻿using Common.Levels;
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
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private SceneSwitcher _sceneSwitcher;
        [SerializeField] private SlowMotionManager _slowMotionEffect;
        [Space(10)]
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ActiveGroupSO activeGroupSo;
        [SerializeField] private MergeItemsStashSO _stashSO;
        [Space(10)]
        [SerializeField] private LevelsRepository _levelsRepository;
        [Space(10)]
        [SerializeField] private MergeTable _mergeTable;
        [SerializeField] private MergeItemViews itemViews;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private ShopItemsViews _shopItemsViews;
        [SerializeField] private HuntersRepository _hunters;
        [SerializeField] private IDataSaver _dataSaver;

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