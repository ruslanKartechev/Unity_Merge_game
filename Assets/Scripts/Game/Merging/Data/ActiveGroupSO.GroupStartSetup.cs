using System.Collections.Generic;

namespace Game.Merging
{
    public partial class ActiveGroupSO
    {
        [System.Serializable]
        public class GroupStartSetup
        {
            public List<Row> rows;
            
            [System.Serializable]
            public class Row
            {
                public bool isAvailable;
                public List<Cell> cells;
            }

            [System.Serializable]
            public class Cell
            {
                public bool purchased;
                public float cost;
                public MergeItemSO item;
            }
        }
    }
}