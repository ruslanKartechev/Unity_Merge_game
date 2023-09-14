using System;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(ActiveGroupSO), fileName = nameof(ActiveGroupSO), order = 0)]
    public class ActiveGroupSO : ScriptableObject, IMergeGridRepository
    {
        [SerializeField] private ActiveGroup _startSetup;
        [NonSerialized] private IActiveGroup _currentSetup = null;

        public IActiveGroup GetSetup()
        {
            if (_currentSetup == null)
            {
                Debug.Log($"merge data null, copying");
                _currentSetup = new ActiveGroup(_startSetup);
            }
            return _currentSetup;
        }
        
        public void SetSetup(IActiveGroup data)
        {
            _currentSetup = data;
        }

        #if UNITY_EDITOR
        public void DebugSetup()
        {
            var data = GetSetup();
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
                    output += $"x{x} => {cell.Item.level}  |  ";
                }
                Debug.Log(output);
            }
        }
        #endif
    }
}