using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    public partial class ActiveGroup
    {
        [System.Serializable]
        public class Cell : IActiveGroupCell
        {
            [SerializeField] private bool purchased;
            [SerializeField] private float cost;
            [SerializeField] private MergeItem item;
            
            public Cell(){}

            public Cell(Cell from)
            {
                purchased = from.purchased;
                cost = from.cost;
                item = new MergeItem(from.item);
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
                get => item;
                set => item = value;
            }
        }
    }
}