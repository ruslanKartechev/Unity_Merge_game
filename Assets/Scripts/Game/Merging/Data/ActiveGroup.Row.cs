using System.Collections.Generic;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    public partial class ActiveGroup
    {
        [System.Serializable]
        public class Row : IActiveGroupRow
        {
            [SerializeField] private bool isAvailable;
            [SerializeField] private List<Cell> elements;
            
            
            public Row()
            {
                elements = new List<Cell>();
            }

            public Row(Row from)
            {
                IsAvailable = from.IsAvailable;
                elements = new List<Cell>(from.elements.Count);
                foreach (var fromElement in from.elements)
                    elements.Add(new Cell(fromElement));
            }

            public void AddCell(Cell cell)
            {
                elements.Add(cell);
            }
            
            public bool IsAvailable
            {
                get => isAvailable;
                set => isAvailable = value;
            }

            public int CellsCount => elements.Count;

            public int ItemsCount
            {
                get
                {
                    var count = 0;
                    foreach (var cell in elements)
                    {
                        if (cell.Item != null)
                            count++;
                    }
                    return count;
                }
            }
            
            public IActiveGroupCell GetCell(int index)
            {
                return elements[index];
            }
        }
    }
}