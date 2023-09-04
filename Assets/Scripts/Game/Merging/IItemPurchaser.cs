using System.Collections.Generic;
using Game.UI.Merging;

namespace Game.Merging
{
    public interface IItemPurchaser
    {
        public void Init(IList<IList<IMergeCell>> cells, IMergeItemSpawner itemSpawner, IMergingPage page);
        bool PurchaseNewItem();
        float GetCost();
    }
}