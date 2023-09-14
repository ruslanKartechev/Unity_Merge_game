using System.Collections.Generic;
using UnityEngine;

namespace Game.Merging
{
    [System.Serializable]
    public class ActiveGroup : IActiveGroup
    {
        [SerializeField] private List<ActiveGroupRow> rows;
        public int RowsCount => rows.Count;
        
        
        public ActiveGroup()
        { }
        public ActiveGroup(ActiveGroup from)
        {
            rows = new List<ActiveGroupRow>(from.rows.Count);
            foreach (var fromRow in from.rows)
                rows.Add(new ActiveGroupRow(fromRow));
        }
        
        public IActiveGroupRow GetRow(int index)
        {
            return rows[index];
        }
        
        
        
        [System.Serializable]
        public class ActiveGroupRow : IActiveGroupRow
        {
            [SerializeField] private bool isAvailable;
            [SerializeField] private List<ActiveGroupCell> elements;
            
            public ActiveGroupRow(){}

            public ActiveGroupRow(ActiveGroupRow from)
            {
                IsAvailable = from.IsAvailable;
                elements = new List<ActiveGroupCell>(from.elements.Count);
                foreach (var fromElement in from.elements)
                    elements.Add(new ActiveGroupCell(fromElement));
            }

            public bool IsAvailable
            {
                get => isAvailable;
                set => isAvailable = value;
            }

            public int CellsCount => elements.Count;
            
            public IActiveGroupCell GetCell(int index)
            {
                return elements[index];
            }
        }
        
        
        
        
        [System.Serializable]
        public class ActiveGroupCell : IActiveGroupCell
        {
            [SerializeField] private bool purchased;
            [SerializeField] private float cost;
            [SerializeField] private MergeItem mergeItem;
            
            public ActiveGroupCell(){}

            public ActiveGroupCell(ActiveGroupCell from)
            {
                purchased = from.purchased;
                cost = from.cost;
                mergeItem = new MergeItem(from.mergeItem);
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

            public MergeItem Item
            {
                get => mergeItem;
                set => mergeItem = value;
            }
        }

    }


}