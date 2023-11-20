using System.Collections.Generic;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public partial class ActiveGroup : IActiveGroup
    {
        [SerializeField] private List<Row> rows;
        public int RowsCount => rows.Count;


        public ActiveGroup()
        {
            rows = new List<Row>();
        }
        
        public ActiveGroup(ActiveGroup from)
        {
            rows = new List<Row>(from.rows.Count);
            foreach (var fromRow in from.rows)
                rows.Add(new Row(fromRow));
        }
        
        public IActiveGroupRow GetRow(int index)
        {
            return rows[index];
        }

        public void ClearCell(int x, int y)
        {
            var cell = GetRow(y).GetCell(x);
            cell.Item = null;
        }

        public int ItemsCount
        {
            get
            {
                var count = 0;
                foreach (var row in rows)
                    count += row.ItemsCount;
                return count;
            }
        }

        public bool Contains(string id)
        {
            foreach (var row in rows)
            {
                for (var i = 0; i < row.CellsCount; i++)
                {
                    var cell = row.GetCell(i);
                    if(row.GetCell(i).Item == null)
                        continue;
                    if (cell.Item.item_id == id)
                        return true;
                }
            }
            return false;
        }

        public void AddRow(Row row)
        {
            rows.Add(row);
        }
    }


}