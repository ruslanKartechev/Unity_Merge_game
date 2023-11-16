using Common.Saving;
using Game.Merging;
using UnityEngine;

namespace Game.Saving
{
    public class SavedDataInitializer : MonoBehaviour, ISavedDataInitializer
    {
        [SerializeField] private bool _applyCheat;
        [SerializeField] private bool _clearData;
        [SerializeField] private bool _hideUnitsUI;
        [Space(10)] 
        [SerializeField] private PlayerData _playerDataCheat;
        [Space(10)] 
        [SerializeField] private bool _useEggsCheat;
        [SerializeField] private bool _isTicking;
        [SerializeField] private bool _wasTicked;
        [SerializeField] private TimerTime _cheatAddedDuration;
        [Space(10)]
        [SerializeField] private IDataSaver _saver;
        
        
        public void InitSavedData()
        {
            if (_clearData)
            {
                _saver.Clear();
                PlayerPrefs.DeleteAll();
            }
            GameState.HideUnitsUI = _hideUnitsUI;
            // Debug.Log($"Hide UI: {_hideUnitsUI}");
            _saver.Load();
            var loaded = _saver.GetLoadedData();
            GC.PlayerData = new PlayerData(loaded.PlayerData);
        
            if (_applyCheat)
                GC.PlayerData = new PlayerData(_playerDataCheat);

            var group = loaded.ActiveGroup;
            if (group is { ItemsCount: 0 })
                group = null;
            Debug.Log("[SavedDataInit] Group data loaded");
            
            GC.ActiveGroupSO.SetGroup(group);
            Debug.Log("[SavedDataInit] Group data SET");
            GC.ItemsStash.Stash = loaded.ItemsStash;
            
            if (_useEggsCheat && _applyCheat)
            {
                for (var i = 0; i <  GC.ItemsStash.SuperEggs.Count; i++)
                {
                    // Debug.Log($"Init cheat egg ${i}");
                    var egg = GC.ItemsStash.SuperEggs[i];
                    var beginTime = new TimerTime(System.DateTime.Now);
                    egg.Init(_isTicking && !_wasTicked, beginTime, beginTime + _cheatAddedDuration, _wasTicked);
                }
            }
            else
            {
                if (loaded.SuperEggsData != null)
                {
                    for (var i = 0; i < loaded.SuperEggsData.Count; i++)
                    {
                        var egg = GC.ItemsStash.SuperEggs[i];
                        egg.Init(loaded.SuperEggsData[i]);
                    }             
                }

                GC.ItemsStash.InitSuperEggs();
            }
       
        }
    }
}