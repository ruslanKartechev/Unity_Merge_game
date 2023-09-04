﻿using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeCell
    {
        int X { get; set; }
        int Y { get; set; }
        float Cost { get; set; }
        // Does it Contain An Item
        bool IsFree { get; }
        // Was It Purchased
        bool IsPurchased { get; }
        // Set Available For Purchase
        void Init(IGridCellData data);
        void SetInactive();
        
        // Unlock
        void Purchase();
        
        void SpawnItem(IMergeItem item);

        void PutItem(IMergeItem item);
        
        /// <summary>
        /// Returns item from the cell and Makes it Free
        /// </summary>
        IMergeItem TakeItem();
        
        /// <summary>
        /// Returns item in the cell. Does NOT make it free
        /// </summary>
        IMergeItem GetItem();
        
        /// <summary>
        /// Destroy Item and make cell free
        /// </summary>
        void RemoveItem();
        
    }
}