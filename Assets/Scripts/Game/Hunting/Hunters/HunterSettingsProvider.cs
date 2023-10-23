using Common.Data;
using Game.Hunting.Hunters.Interfaces;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HunterSettingsProvider), fileName = nameof(HunterSettingsProvider), order = 0)]
    public class HunterSettingsProvider : ScriptableObject
    {
        [SerializeField] private CollectionDataByID<HunterSettings_Land> _landSettings;
        [SerializeField] private CollectionDataByID<HunterSettingsAir> _airSettings;
        [SerializeField] private CollectionDataByID<HunterSettingsWaterWater> _waterSettings;
        [SerializeField] private CollectionDataByID<HunterSettings_Kong> _superSettings;
        
        
        public void Init()
        {
            _landSettings.InitTable();
            _airSettings.InitTable();
            _waterSettings.InitTable();
        }
        
        public object GetSettings(MergeItem item)
        {
            switch (item.class_id)
            {
                case MergeItem.LandClass:
                    return GetSettingsLand(item.item_id);
                case MergeItem.AirClass:
                    return GetSettingsAir(item.item_id);
                case MergeItem.WaterClass:
                    return GetSettingsWater(item.item_id);
                case MergeItem.SuperClass:
                    return GetSettingsKong(item.item_id);
            }
            return null;
        }

        
        public IHunterSettings GetSettingsLand(string id)
        {
            return _landSettings.GetObject(id);
        }
        
        public IHunterSettings_Air GetSettingsAir(string id)
        {
            return _airSettings.GetObject(id);
        }
        
        
        public IHunterSettings_Water GetSettingsWater(string id)
        {
            return _waterSettings.GetObject(id);
        }

        public IHunterSettings_Kong GetSettingsKong(string id)
        {
            return _superSettings.GetObject(id);
        }

        
        #if UNITY_EDITOR
        [ContextMenu("Fill All Items IDs 0-4")]
        public void FillItems()
        {
            var min = 0;
            var max = 5;
            FillClass(_landSettings, min, max, "land");
            FillClass(_waterSettings, min, max, "water");
            FillClass(_airSettings, min, max, "air");
            FillClass(_superSettings, 0, 1, "super");
        }

        [ContextMenu("ClearAllClasses")]
        public void ClearAllClasses()
        {
            _landSettings.Clear();
            _waterSettings.Clear();
            _airSettings.Clear();
            _superSettings.Clear();
        }
        
        private void FillClass<T>(CollectionDataByID<T> dataList, int min, int max, string id)
        {
            for (var i = min; i < max; i++)
            {
                var itemId = id + $"_{i}";
                dataList.AddItem(new DataByID<T>(default, itemId));
            }
           
        }
        #endif
    }
}