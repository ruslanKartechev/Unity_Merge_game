using Common.Saving;
using UnityEngine;

namespace Game.Saving
{
    public class SavedDataInitializer : MonoBehaviour, ISavedDataInitializer
    {
        [SerializeField] private bool _applyCheat;
        [Space(4)]
        [SerializeField] private float _cheatMoney;
        [SerializeField] private float _cheatCrystals;
        [SerializeField] private int _cheatLevelInd;
        [SerializeField] private int _cheatLevelTotal;
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
            _playerData.Crystals = _cheatCrystals;

            if (_applyCheat)
            {
                _playerData.Money = _cheatMoney;
                _playerData.Crystals = _cheatCrystals;
                _playerData.LevelIndex = _cheatLevelInd;
                _playerData.LevelTotal = _cheatLevelTotal;
            }
            GC.ActiveGridSO.SetSetup(loaded.MergeGridData());
        }
        
        
    }
}