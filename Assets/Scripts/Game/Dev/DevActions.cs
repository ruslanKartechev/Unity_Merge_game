using Game.Merging;
using Game.UI;
using UnityEngine;
using Utils;

namespace Game.Dev
{
    public class DevActions : MonoBehaviour
    {
        [SerializeField] private InputSettings _inputSettings;
        public static DevActions Instance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void NextLevel()
        {
            CLog.LogWHeader("DEV", "Next level", "r", "w");
            Game.GC.LevelManager.LoadNext();            
        }

        public void PrevLevel()
        {
            CLog.LogWHeader("DEV", "Prev level", "r", "w");
            Game.GC.LevelManager.LoadPrev();
        }

        public void AddMoney()
        {
            CLog.LogWHeader("DEV", "Add money", "r", "w");
            Game.GC.PlayerData.Money += 1000;
            UIC.Money?.UpdateCount();
        }

        public void AddCrystals()
        {
            CLog.LogWHeader("DEV", "Add crystals", "r", "w");
            Game.GC.PlayerData.Crystal += 1000;
            UIC.Crystals?.UpdateCount();
        }

        public void ClearSavedData()
        {
            CLog.LogWHeader("DEV", "Clearing saved data", "r", "w");
            Game.GC.DataSaver.Clear();   
        }

        public float GetMaxSensX() => _inputSettings.GetMaxSensX();
        public float GetMaxSensY() => _inputSettings.GetMaxSensY();

        public void SetMaxSensX(float value)
        {
            _inputSettings.SetSensitivityX(value);
        }
        
        public void SetMaxSensY(float value)
        {
            _inputSettings.SetSensitivityY(value);
        }


    }
}