using Common;
using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Common.SlowMotion;
using Game.Hunting;
using Game.Hunting.UI;
using Game.Merging;
using Game.Shop;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class GCLocator : MonoBehaviour, IGlobalContainerLocator
    {
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private SceneSwitcher _sceneSwitcher;
        [SerializeField] private SlowMotionManager _slowMotionEffect;
        [Space(10)]
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

        public void InitContainer()
        {
            GC.PlayerData = _playerData.Data;
            GC.SceneSwitcher = _sceneSwitcher;
            GC.DataSaver = _dataSaver;
            GC.LevelManager = _levelManager;
            GC.HuntersRepository = _hunters;
            GC.ActiveGroupSO = _activeGroupSo;
            GC.LevelRepository = _levelsRepository;
            GC.ItemsStash = _stashSO;
            GC.ItemViews = itemViews;
            GC.MergeTable = _mergeTable;
            GC.ShopItems = _shopItems;
            GC.ShopItemsViews = _shopItemsViews;
            GC.SlowMotion = _slowMotionEffect;
            GC.Input = _input;
            _hunters.Init();
            _shopItemsViews.Init();
            GC.ShopSettingsRepository = _shopItems;
            _particlesRepository.Init();
            GC.ParticlesRepository = _particlesRepository;
            GC.UIManager = _uiManager;
        }
        
        
        #if UNITY_EDITOR
        [Space(22)]
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


    #if UNITY_EDITOR
    [CustomEditor(typeof(GCLocator))]
    public class GCLocatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as GCLocator;
            if (GUILayout.Button("Release", GUILayout.Width(100)))
                   me.ReleaseMode();
            if (GUILayout.Button("Debug", GUILayout.Width(100)))
                me.DebugMode();
        }
    }
    #endif
    
}