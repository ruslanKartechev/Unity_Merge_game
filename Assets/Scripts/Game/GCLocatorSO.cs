using Common.Levels;
using Common.Saving;
using Game.Hunting;
using Game.Hunting.Hunters;
using Game.Hunting.UI;
using Game.Merging;
using Game.Shop;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/" + nameof(GCLocatorSO), fileName = nameof(GCLocatorSO), order = 0)]
    public class GCLocatorSO : ScriptableObject
    {
        [SerializeField] private PlayerDataSO _playerData;
        [SerializeField] private ActiveGroupSO _activeGroupSo;
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
        [SerializeField] private ParticlesRepository _particlesRepository;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private HunterSettingsProvider _hunterSettingsProvider;
        
        public void InitContainer()
        {
            GC.PlayerData = _playerData.Data;
            GC.DataSaver = _dataSaver;

            GC.ActiveGroupSO = _activeGroupSo;
            GC.LevelRepository = _levelsRepository;
            GC.ItemsStash = _stashSO;
            GC.ItemViews = itemViews;
            GC.MergeTable = _mergeTable;
            GC.ShopItems = _shopItems;
            GC.ParticlesRepository = _particlesRepository;
            GC.UIManager = _uiManager;
            
            GC.ShopItemsViews = _shopItemsViews;
            _shopItemsViews.Init();
            
            GC.ShopSettingsRepository = _shopItems;
            _particlesRepository.Init();
            
            GC.HuntersRepository = _hunters;
            _hunters.Init();
            
            GC.HunterSettingsProvider = _hunterSettingsProvider;
            _hunterSettingsProvider.Init();
        }
        
        
#if UNITY_EDITOR
        [Space(30)]
        [Header("EDITOR MODE")]
        [SerializeField] private ActiveGroupSO _activeGroupSo_Debug;
        [SerializeField] private MergeItemsStashSO _stashSO_Debug;
        [SerializeField] private ActiveGroupSO activeGroupSo_Release;
        [SerializeField] private MergeItemsStashSO _stashSO_Release;

        public void ReleaseMode()
        {
            _activeGroupSo = activeGroupSo_Release;
            _stashSO = _stashSO_Release;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void DebugMode()
        {
            _activeGroupSo = _activeGroupSo_Debug;
            _stashSO = _stashSO_Debug;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}