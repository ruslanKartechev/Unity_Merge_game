using Common.Utils;
using Game.UI.Map;
using UnityEngine;

namespace Game.UI.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(UIManager), fileName = nameof(UIManager), order = 0)]
    public class UIManager : ScriptableObject
    {
        [SerializeField] private LevelsMap _winLevelMap_prefab;
        [SerializeField] private WinPopup _winPopup_prefab;
        [SerializeField] private LoosePopup _loosePopup_prefab;
        [SerializeField] private DarkeningUI _darkeningUI_prefab;
        [SerializeField] private BonusEggPopup _bonusEgg_prefab;
        
        private LevelsMap _winLevelMap_instance;
        private WinPopup _winLevelPopup_instance;
        private LoosePopup _looseLevelPopup_instance;
        private DarkeningUI _darkeningUI_instance;
        private BonusEggPopup _bonusEgg_instance;

        
        public LevelsMap WinLevelMap
        {
            get
            {
                if (_winLevelMap_instance != null)
                    return _winLevelMap_instance;   
                CLog.LogWHeader("UIManager", "Spawning win  level map", "r", "w");
                _winLevelMap_instance = Instantiate(_winLevelMap_prefab);
                return _winLevelMap_instance;
            }
        }
        
        public WinPopup WinPopup
        {
            get
            {
                if (_winLevelPopup_instance != null)
                    return _winLevelPopup_instance;   
                CLog.LogWHeader("UIManager", "Spawning win level", "r", "w");
                _winLevelPopup_instance = Instantiate(_winPopup_prefab);
                return _winLevelPopup_instance;
            }
        }

        public LoosePopup LoosePopup
        {
            get
            {
                if (_looseLevelPopup_instance != null)
                    return _looseLevelPopup_instance;   
                CLog.LogWHeader("UIManager", "Spawning fail level", "r", "w");
                _looseLevelPopup_instance = Instantiate(_loosePopup_prefab);
                return _looseLevelPopup_instance;
            }
        }
        
     
        public DarkeningUI Darkening
        {
            get
            {
                if (_darkeningUI_instance != null)
                    return _darkeningUI_instance;   
                CLog.LogWHeader("UIManager", "Spawning Darkening", "r", "w");
                _darkeningUI_instance = Instantiate(_darkeningUI_prefab);
                return _darkeningUI_instance;
            }
        }
        
         
        public BonusEggPopup BonusEggPopup
        {
            get
            {
                if (_darkeningUI_instance != null)
                    return _bonusEgg_instance;   
                CLog.LogWHeader("UIManager", "Spawning Bonus Egg", "r", "w");
                _bonusEgg_instance = Instantiate(_bonusEgg_prefab);
                return _bonusEgg_instance;
            }
        }
    }
}