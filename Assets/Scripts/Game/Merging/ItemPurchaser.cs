using System.Collections.Generic;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class ItemPurchaser : MonoBehaviour, IItemPurchaser
    {
        [SerializeField] private MergeSettings _settings;
        private IList<IList<IMergeCell>> _cells;
        private IMergingPage _page;
        private IMergeItemSpawner _mergeItemSpawner;
        
        public float GetCost() => _settings.FirstLevelCost();
        
        public void Init(IList<IList<IMergeCell>> cells, IMergeItemSpawner mergeItemSpawner, IMergingPage page)
        {
            _cells = cells;
            _page = page;
            _mergeItemSpawner = mergeItemSpawner;
        }

        public bool PurchaseNewItem()
        {
            var money = Container.PlayerData.Money;
            var cost = _settings.FirstLevelCost();
            if (money < cost)
            {
                CLog.LogWHeader("ItemsPurchaser", $"Not enough money: {money} < {cost}", "r", "w");
                return false;
            }
            
            foreach (var row in _cells)
            {
                foreach (var cell in row)
                {
                    if (cell.IsPurchased == false)
                        return false;

                    if (cell.IsFree && cell.IsPurchased)
                    {
                        _mergeItemSpawner.SpawnItem(cell, 0);
                        Container.PlayerData.Money -= cost;
                        _page.UpdateMoney();
                        return true;
                    }
                }
            }
            return false;
        }
        
        
    }
}
