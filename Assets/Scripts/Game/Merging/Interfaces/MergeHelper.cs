using System.Collections.Generic;
using Game.Hunting;
using Game.Hunting.Hunters.Interfaces;

namespace Game.Merging
{
    public static class MergeHelper
    {
        public static List<MergeItem> GetAllItems(IActiveGroup pack)
        {
            var list = new List<MergeItem>();
            for (int y = 0; y < pack.RowsCount; y++)
            {
                var row = pack.GetRow(y);
                for (var x = 0; x < row.CellsCount; x++)
                {
                    var item = row.GetCell(x).Item;
                    if (MergeItem.Empty(item))
                        continue;
                    list.Add(item);
                }
            }

            return list;
        }
        
        public static float CalculatePowerUs(IActiveGroup pack)
        {
            var power = 0f;
            for (int y = 0; y < pack.RowsCount; y++)
            {
                var row = pack.GetRow(y);
                for (var x = 0; x < row.CellsCount; x++)
                {
                    var item = row.GetCell(x).Item;
                    if (MergeItem.Empty(item))
                        continue;
                    var settings = (IHunterSettings)GC.HunterSettingsProvider.GetSettings(item);
                    power += settings.Damage;
                }
            }
            return power;
        }
        
        
        public static float CalculatePowerEnemy(ILevelSettings level)
        {
            var power = 0f;
            for (int i = 0; i < level.PreySettingsList.Count; i++)
            {
                power += level.PreySettingsList[i].Health;
            }
            return power;
        }
    }
}