using UnityEngine;

namespace Game.UI.Merging
{
    public static class MergeAreaSizeAdjuster
    {
        private const int MinCount = 8;
        private const int RowCount = 4;
        private const float SizePerRow = 200;
        private const float MinSize = 500;
        
        
        public static void AdjustSize(RectTransform rect, int itemsCount)
        {
            if (itemsCount <= MinCount)
            {
                var delta = rect.sizeDelta;
                delta.y = MinSize;
                rect.sizeDelta = delta;
                // Debug.Log($"Count: {itemsCount}, normal size {delta.y}");
                return;
            }
            {
                var overflowCount = itemsCount - MinCount;
                var addRowsCount = (overflowCount % RowCount) + 1;
                var delta = rect.sizeDelta;
                delta.y = MinSize + addRowsCount * SizePerRow;
                rect.sizeDelta = delta;            
                // Debug.Log($"Count: {itemsCount}, overflow: {overflowCount}, size: {delta.y}");
            }
        }
    }
}