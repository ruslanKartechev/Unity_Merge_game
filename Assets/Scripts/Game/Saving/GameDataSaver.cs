using System;
using System.Collections.Generic;
using System.IO;
using Common.Saving;
using Game.Merging;
using UnityEngine;
using Utils;

namespace Game.Saving
{
    [CreateAssetMenu(menuName = "SO/" + nameof(GameDataSaver), fileName = nameof(GameDataSaver), order = 0)]
    public class GameDataSaver : IDataSaver
    {
        [NonSerialized] private SavedData _loadedData;

        public override ISavedData GetLoadedData()
        {
            if(_loadedData == null)
                CLog.LogWHeader("DataSaver", "Null saved data!", "r");
            return _loadedData;
        }

        public override void Load()
        {
            _loadedData = new SavedData();
            if (File.Exists(Path))
            {
                var fileContents = File.ReadAllText(Path);
                _loadedData = JsonUtility.FromJson<SavedData>(fileContents);
                if (_loadedData == null)
                    _loadedData = new SavedData();
            }
            else
            {
                _loadedData = new SavedData();
            }
        }

        public override void Save()
        {
            var playerData = GC.PlayerData;
            var activeGroup = (ActiveGroup)GC.ActiveGroupSO.Group();
            var stash = GC.ItemsStash.Stash;
            
            var superEggs = GC.ItemsStash.SuperEggs;
            var eggData = new List<SuperEggSaveData>(superEggs.Count);
            foreach (var egg in superEggs)
                eggData.Add(egg.SaveData);
            
            var gameData = new SavedData(playerData, activeGroup, stash, eggData);
            var jsonString = JsonUtility.ToJson(gameData);
            File.WriteAllText(Path, jsonString);
        }
        
        
        public override void Clear()
        {
            PlayerPrefs.DeleteAll();
            if(Application.isPlaying)
                CLog.LogWHeader("DataSaver", "Saved Data Cleared!", "w");
            File.Delete(Path);
            _loadedData = null;
        }

        public void LogPath()
        {
            Debug.Log($"Path to saves\n {Path}");
        }
    }
}