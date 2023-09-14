using System;
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
            if (playerData == null)
            {
                CLog.LogWHeader("DataSaver", "No player data, cannot save!", "r");
                return;
            }
            var data = new SavedData(playerData);
            data.gridData = (ActiveGroup)GC.GridRepository.GetSetup();
            
            var jsonString = JsonUtility.ToJson(data);
            File.WriteAllText(Path, jsonString);
        }

        public override void Clear()
        {
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