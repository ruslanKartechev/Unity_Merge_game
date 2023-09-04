using System.Collections;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class MergeInput : MonoBehaviour, IMergeInput
    {
        [SerializeField] private float _upOffset;
        [SerializeField] private InputSettings _settings;
        private IMergingPage _mergingPage;
        private IMergeItemSpawner _mergeItemSpawner;
        private Coroutine _inputTaking;
        private MovingItem _draggedItem;
        private Camera _camera;
        private Vector3 _mousePos;
        
        public void Init(IMergingPage page, IMergeItemSpawner spawner)
        {
            _mergingPage = page;
            _mergeItemSpawner = spawner;
            _camera = Camera.main;
        }

        public void Activate()
        {
            Stop();
            _inputTaking = StartCoroutine(InputTaking());
        }

        public void Stop()
        {
            if(_inputTaking != null)
                StopCoroutine(_inputTaking);
        }

        private IEnumerator InputTaking()
        {
            var oldPos = Input.mousePosition;
            var newPos = oldPos;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _mousePos = oldPos = newPos = Input.mousePosition;
                    var cell = TryGetCell();
                    if (cell != null)
                    {
                        if (_draggedItem == null)
                        {
                            if (!TryPurchase(cell))
                                TryTake(cell);
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (_draggedItem != null)
                    {
                        var cell = TryGetCell();
                        if (cell != null)
                            TryPut(cell);
                        else
                            DropBack();         
                    }
                }
                
                if (Input.GetMouseButton(0))
                {
                    _mousePos = newPos = Input.mousePosition;
                    if (_draggedItem != null)
                        TryMove();
                }

                oldPos = newPos;
                yield return null;
            }      
        }

        
        private IMergeCell TryGetCell()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100, _settings.mergingMask))
                return null;
            var cell = hit.collider.gameObject.GetComponent<IMergeCell>();
            return cell;
        }
        
        private bool TryPurchase(IMergeCell cell)
        {
            if (cell.IsPurchased)
                return false;
            var cost = cell.Cost;
            if (cost > Container.PlayerData.Money)
            {
                CLog.LogWHeader("MergeInput", $"Not enough money to purchase the cell for {cost}", "w");
                return false;
            }
            Container.PlayerData.Money -= cost;
            cell.Purchase();
            _mergingPage.UpdateMoney();
            return true;
        }

        private void TryTake(IMergeCell cell)
        {
            var item = cell.TakeItem();
            if (item == null)
                return;
            _draggedItem = new MovingItem(cell, item);
            _draggedItem.item.OnPicked();
        }

        private void TryPut(IMergeCell cell)
        {
            if (cell.IsPurchased == false)
            {
                DropBack();
                return;
            }
            if (!cell.IsFree)
            {
                if (TryMerge(cell))
                {
                    _draggedItem = null;
                    return;
                }
                Swap(cell);
                return;
            }
            cell.PutItem(_draggedItem.item);
            Release();
        }

        private void TryMove()
        {
            var ray = _camera.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _settings.groundMask))
                _draggedItem.item.SetPosition(hit.point + Vector3.up * _upOffset);
        }

        private bool TryMerge(IMergeCell cell)
        {
            var level = cell.GetItem().ItemLevel;
            if (level >= _mergeItemSpawner.MaxLevel)
                return false;
            if (level != _draggedItem.item.ItemLevel)
                return false;
            cell.TakeItem().Destroy();
            _draggedItem.item.Destroy();
            _mergeItemSpawner.SpawnItem(cell, level + 1);
            return true;
        }

        private void Swap(IMergeCell cell)
        {
            var cellItem = cell.TakeItem();
            _draggedItem.cell.PutItem(cellItem);
            cell.PutItem(_draggedItem.item);
            Release();
        }
        
        private void DropBack()
        {
            if (_draggedItem == null)
                return;
            _draggedItem.cell.PutItem(_draggedItem.item);
            Release();
        }

        private void Release()
        {
            _draggedItem.item.OnReleased();
            _draggedItem = null;   
        }

        private class MovingItem
        {
            public IMergeCell cell;
            public IMergeItem item;
            
            public MovingItem(IMergeCell fromCell, IMergeItem item)
            {
                this.item = item;
                cell = fromCell;
            }   
            
        }
    }
}