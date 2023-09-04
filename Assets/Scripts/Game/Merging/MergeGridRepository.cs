using System;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeGridRepository), fileName = nameof(MergeGridRepository), order = 0)]
    public class MergeGridRepository : ScriptableObject, IMergeGridRepository
    {
        [SerializeField] private MergeGridData _startSetup;
        [NonSerialized] private IMergeGridData _currentSetup;

        public IMergeGridData GetSetup()
        {
            if (_currentSetup == null)
            {
                Debug.Log($"merge data null, copying");
                _currentSetup = new MergeGridData(_startSetup);
            }
            return _currentSetup;
        }
        
        public void SetSetup(IMergeGridData data)
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
                    output += $"x{x} => {cell.SpawnItemLevel}  |  ";
                }
                Debug.Log(output);
            }
        }
        #endif
    }
}