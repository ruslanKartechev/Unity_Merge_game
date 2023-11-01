using System.Collections.Generic;

namespace Game.Merging
{
    public interface IActiveGroup
    {
        int RowsCount { get; }
        IActiveGroupRow GetRow(int index);
        void ClearCell(int x, int y);
        int ItemsCount { get; }
        bool Contains(string id);
    }

    public static class GroupHelper
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
    }
}