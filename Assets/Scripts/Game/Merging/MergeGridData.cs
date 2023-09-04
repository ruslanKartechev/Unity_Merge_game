using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class MergeGridData : IMergeGridData
    {
        [SerializeField] private List<MergeGridRow> rows;
        public int RowsCount => rows.Count;
        
        
        public MergeGridData()
        { }
        public MergeGridData(MergeGridData from)
        {
            rows = new List<MergeGridRow>(from.rows.Count);
            foreach (var fromRow in from.rows)
                rows.Add(new MergeGridRow(fromRow));
        }
        
        public IMergeGridRow GetRow(int index)
        {
            return rows[index];
        }
        
        
        
        [System.Serializable]
        public class MergeGridRow : IMergeGridRow
        {
            [SerializeField] private bool isAvailable;
            [SerializeField] private List<GridCellData> elements;
            
            public MergeGridRow(){}

            public MergeGridRow(MergeGridRow from)
            {
                IsAvailable = from.IsAvailable;
                elements = new List<GridCellData>(from.elements.Count);
                foreach (var fromElement in from.elements)
                    elements.Add(new GridCellData(fromElement));
            }

            public bool IsAvailable
            {
                get => isAvailable;
                set => isAvailable = value;
            }

            public int CellsCount => elements.Count;
            
            public IGridCellData GetCell(int index)
            {
                return elements[index];
            }
        }
        
        
        
        
        [System.Serializable]
        public class GridCellData : IGridCellData
        {
            [SerializeField] private bool purchased;
            [SerializeField] private float cost;
            [SerializeField] private int itemLevel = -1;
            
            public GridCellData(){}

            public GridCellData(GridCellData from)
            {
                purchased = from.purchased;
                cost = from.cost;
                itemLevel = from.itemLevel;
            }

            public bool Purchased
            {
                get => purchased;
                set => purchased = value;
            }

            public float Cost
            {
                get => cost;
                set => cost = value;
            }

            public int SpawnItemLevel
            {
                get => itemLevel;
                set => itemLevel = value;
            }
        }

    }


}