using Game.Merging;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.UI.Merging
{
    public interface IGroupCellView
    {
        int X { get; set; }
        int Y { get; set; }
        float Cost { get; set; }
        // Does it Contain An Item
        bool IsFree { get; }
        // Was It Purchased
        bool IsPurchased { get; }
        // Set Available For Purchase
        void Init(IActiveGroupCell data);
        void SetInactive();
        
        // Unlock
        void Purchase();
        
        void SpawnItem(IMergeItemView itemView, MergeItem item);

        void PutItem(IMergeItemView item);
        MergeItem GetItem();
        
        /// <summary>
        /// Returns item from the cell and Makes it Free
        /// </summary>
        IMergeItemView PickItemView();
        
        /// <summary>
        /// Returns item in the cell. Does NOT make it free
        /// </summary>
        IMergeItemView GetItemView();
        
        /// <summary>
        /// Destroy Item and make cell free
        /// </summary>
        void RemoveItem();

        Vector3 GetPosition();
    }
}