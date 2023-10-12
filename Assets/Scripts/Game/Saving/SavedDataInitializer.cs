using Common.Saving;
using Game.Merging;
using UnityEngine;

namespace Game.Saving
{
    public class SavedDataInitializer : MonoBehaviour, ISavedDataInitializer
    {
        [SerializeField] private bool _applyCheat;
        [Space(10)] 
        [SerializeField] private PlayerData _playerDataCheat;
        [Space(10)] 
        [SerializeField] private bool _useEggsCheat;
        [SerializeField] private TimerTime _cheatAddedDuration;
        [SerializeField] private bool _isTicking;
        [Space(10)]
        [SerializeField] private IDataSaver _saver;
        
        
        public void InitSavedData()
        {
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