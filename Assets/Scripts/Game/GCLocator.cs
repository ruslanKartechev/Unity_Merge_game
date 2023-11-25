using Common;
using Common.Levels;
using Common.Saving;
using Common.Scenes;
using Common.SlowMotion;
using Game.Hunting;
using Game.Hunting.UI;
using Game.Merging;
using Game.Shop;
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
            try
            {
                if(_playerData != null)
                    GC.PlayerData = _playerData.Data;
            }
            catch (System.Exception ex)
            {
                Log($"Exception PlayerData!! {ex.Message}");
            }
            try
            {
                GC.SceneSwitcher = _sceneSwitcher;
            }
            catch (System.Exception ex)
            {
                Log($"Exception SceneSwitcher!! {ex.Message}");
            }
            
            try
            {
                GC.DataSaver = _dataSaver;
            }
            catch (System.Exception ex)
            {
                Log($"Exception DataSaver!! {ex.Message}");
            }
            
            try
            {
                GC.LevelManager = _levelManager;
            }
            catch (System.Exception ex)
            {
                Log($"Exception LevelManager!! {ex.Message}");
            }
            
            try
            {
                GC.HuntersRepository = _hunters;
            }
            catch (System.Exception ex)
            {
                Debug.Log($"Exception HuntersRepo!! {ex.Message}");
            }
            
            try
            {
                GC.ActiveGroupSO = _activeGroupSo;
            }
            catch (System.Exception ex)
            {
                Log($"Exception ActiveGroup!! {ex.Message}");
            }
            try
            {
                GC.LevelRepository = _levelsRepository;
            }
            catch (System.Exception ex)
            {
                Log($"Exception LevelsRepo!! {ex.Message}");
            }
            try
            {
                GC.ItemsStash = _stashSO;
            }
            catch (System.Exception ex)
            {
                Log($"Exception ItemsStash!! {ex.Message}");
            }
            try
            {
                Log("Init merge item views");
                GC.ItemViews = itemViews;
                if(itemViews != null)
                    itemViews.Init();
            }
            catch (System.Exception ex)
            {
                Log($"Exception ItemViews!! {ex.Message}");
            }
            try
            {
                GC.MergeTable = _mergeTable;
            }
            catch (System.Exception ex)
            {
                Log($"Exception MergeTable!! {ex.Message}");
            }
            try
            {
                GC.ShopItems = _shopItems;
            }
            catch (System.Exception ex)
            {
                Log($"Exception ShopItems!! {ex.Message}");
            }

            try
            {
                Log("Init shop item views");
                GC.ShopItemsViews = _shopItemsViews;
            }
            catch (System.Exception ex)
            {
                Log($"Exception ShopItemViews!! {ex.Message}");
            }
            
            
            try
            {
                GC.SlowMotion = _slowMotionEffect;
            }
            catch (System.Exception ex)
            {
                Log($"Exception SlowMotion!! {ex.Message}");
            }
            try
            {
                GC.Input = _input;
            }
            catch (System.Exception ex)
            {
                Log($"Exception Input!! {ex.Message}");
            }
            try
            {
                if(_hunters != null)
                    _hunters.Init();
            }
            catch (System.Exception ex)
            {
                Log($"Exception Init Huners Repo!! {ex.Message}");
            }
            try
            {
                if(_shopItemsViews != null)
                    _shopItemsViews.Init();
            }
            catch (System.Exception ex)
            {
                Log($"Exception Init ShopItemViews!! {ex.Message}");
            }
            try
            {
                GC.ShopSettingsRepository = _shopItems;
            }
            catch (System.Exception ex)
            {
                Log($"Exception ShopSettingsRepo!! {ex.Message}");
            }
            try
            {
                GC.ParticlesRepository = _particlesRepository;
                if(_particlesRepository != null)
                    _particlesRepository.Init();
            }
            catch (System.Exception ex)
            {
                Log($"Exception ParticlesRepo!! {ex.Message}");
            }
            try
            {
                GC.UIManager = _uiManager;
            }
            catch (System.Exception ex)
            {
                Log($"Exception UI Manager!! {ex.Message}");
            }
        }

        private void Log(string msg)
        {
            Debug.Log(msg);
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
}