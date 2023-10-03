using Common.Saving;
using Game.Merging;
using UnityEngine;

namespace Game.Saving
{
    public class SavedDataInitializer : MonoBehaviour, ISavedDataInitializer
    {
        [SerializeField] private bool _applyCheat;
        [Space(5)] 
        [SerializeField] private bool _cheatTutorPlayed_Attack;
        [SerializeField] private bool _cheatTutorPlayed_Merge;
        [Space(5)]
        [SerializeField] private float _cheatMoney;
        [SerializeField] private float _cheatCrystals;
        [Space(5)] 
        [SerializeField] private int _cheatLevelInd;
        [SerializeField] private int _cheatLevelTotal;
        [Space(10)] 
        [SerializeField] private bool _useEggsCheat;
        [SerializeField] private TimerTime _cheatAddedDuration;
        [SerializeField] private bool _isTicking;
        [Space(10)]
        [SerializeField] private IDataSaver _saver;
        [SerializeField] private PlayerData _playerData;
        
        
        public void InitSavedData()
        {
            _saver.Load();
            var loaded = _saver.GetLoadedData();
            _playerData.Money = loaded.Money();
            _playerData.Crystals = loaded.Crystal();
            _playerData.LevelIndex = loaded.LevelIndex();
            _playerData.LevelTotal = loaded.LevelTotal();
            _playerData.Crystals = loaded.Crystal();

            if (_applyCheat)
            {
                _playerData.Crystals = _cheatCrystals;
                _playerData.Money = _cheatMoney;
                _playerData.Crystals = _cheatCrystals;
                _playerData.LevelIndex = _cheatLevelInd;
                _playerData.LevelTotal = _cheatLevelTotal;
                _playerData.TutorPlayed_Attack = _cheatTutorPlayed_Attack;
                _playerData.TutorPlayed_Merge = _cheatTutorPlayed_Merge;
            }
            
            GC.ActiveGroupSO.SetSetup(loaded.ActiveGroup);
            GC.ItemsStash.Stash = loaded.ItemsStash;
            
            if (_useEggsCheat && _applyCheat)
            {
                for (var i = 0; i <  GC.ItemsStash.SuperEggs.Count; i++)
                {
                    Debug.Log($"Init cheat egg ${i}");
                    var egg = GC.ItemsStash.SuperEggs[i];
                    var beginTime = new TimerTime(System.DateTime.Now);
                    egg.Init(_isTicking, beginTime, beginTime + _cheatAddedDuration);
                }
            }
            else
            {
                if (loaded.SuperEggsData == null)
                    return;
                for (var i = 0; i < loaded.SuperEggsData.Count; i++)
                {
                    var egg = GC.ItemsStash.SuperEggs[i];
                    egg.Init(loaded.SuperEggsData[i]);
                }         
            }
       
        }
    }
}