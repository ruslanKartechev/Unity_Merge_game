using System.Collections.Generic;
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

        public void AddRow(Row row)
        {
            rows.Add(row);
        }
    }


}