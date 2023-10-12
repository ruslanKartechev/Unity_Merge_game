using System;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ActiveGroupSO), fileName = nameof(ActiveGroupSO), order = 0)]
    public partial class ActiveGroupSO : ScriptableObject, IActiveGroupSO
    {
        [SerializeField] private GroupStartSetup _startSetup;
        [NonSerialized] private IActiveGroup _group = null;

        public IActiveGroup Group()
        {
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
                return MakeStartGroup();
            #endif
            
            if (_group == null)
            {
                CLog.LogRed($"_activeGroup == null, creating new from _startSetup");
                _group = MakeStartGroup();
            }
            return _group;
        }
        
        public void SetGroup(IActiveGroup group)
        {
            _group = group;
            // CLog.LogBlue($"SET ACTIVE GROUP. Items count: {(_group==null ? "NULL" : _group.ItemsCount)}");
            if (_group == null || _group.ItemsCount == 0)
            {
                // CLog.LogBlue($"_activeGroup == null, creating new from _startSetup");
                _group = MakeStartGroup();
            }
        }

        #if UNITY_EDITOR
        public void DebugSetup()
        {
            var data = Group();
            if (data.RowsCount == 0)
                return;
            if (data.GetRow(0) == null)
                return;
            
            for (var rowInd = 0; rowInd < data.RowsCount; rowInd++)
            {
                var row = data.GetRow(rowInd);
                if(row.IsAvailable == false)
                    continue;
                Debug.Log($"ROW {rowInd} ************");
                var output = "";
                for (var x = 0; x < row.CellsCount; x++)
                {
                    var cell = row.GetCell(x);
                    if (cell.Item == null)
                    {
                        output += $"x{x} => null | ";
                    }
                    else
                    {
                        output += $"x{x} => LVL_{cell.Item.level} | ";
                    }
                }
                Debug.Log(output);
            }
        }
        #endif

        private IActiveGroup MakeStartGroup()
        {
            var group = new ActiveGroup();
            foreach (var fromRow in _startSetup.rows)
            {
                var row = new ActiveGroup.Row();
                row.IsAvailable = fromRow.isAvailable;
                foreach (var fromCell in fromRow.cells)
                {
                    var cell = new ActiveGroup.Cell()
                    {
                        Cost = fromCell.cost,
                        Purchased = fromCell.purchased,
                        Item = fromCell.item == null ? null : new MergeItem(fromCell.item.Item)
                    };
                    row.AddCell(cell);
                }
                group.AddRow(row);
            }
            return group;
        }
    }
}