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
        [SerializeField] private UIRaycaster _uiRaycaster;
        [SerializeField] private GroupGridBuilder _gridBuilder;
        private IMergingPage _mergingPage;
        private IMergeItemSpawner _mergeItemSpawner;
        private Coroutine _inputTaking;
        private DraggedItem _draggedItem;
        private Camera _camera;
        private Vector3 _mousePos;
        private bool _isActive;
        private IMergeInputUI _mergeInputUI;


        public void Init(IMergingPage page, IMergeItemSpawner spawner, IMergeInputUI mergeInputUI)
        {
            _mergingPage = page;
            _mergeItemSpawner = spawner;
            _camera = Camera.main;
            _mergeInputUI = mergeInputUI;
            _draggedItem = new DraggedItem();
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

        public void TakeItem(MergeItem item)
        {
            Debug.Log("called to spawn item");
            PutItemBack();
            var instance = GC.ItemViewRepository.GetPrefab(item.item_id);
            var view = instance.GetComponent<IMergeItemView>();
            view.Item = item;
            var cell = GetFreeCell();
            _draggedItem.Init(cell, view);
            
        }
        
        private void Click()
        {
            if (!_draggedItem.IsFree) 
                return;
            var cell = TryGetCell();
            if (cell == null)
                return;
            if (!TryPurchase(cell))
                PickFromCell(cell);
        }
        

        private IEnumerator InputTaking()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _mousePos = Input.mousePosition;
                    Click();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_isActive)
                        Release();
                }
                if (Input.GetMouseButton(0))
                {
                    _mousePos = Input.mousePosition;
                    if (_isActive)
                        Move();
                }
                yield return null;
            }      
        }
        
        private void Release()
        {
            var cell = TryGetCell();
            if (cell != null)
                PutToCell(cell);
            else
                PutItemBack();   
        }
        
        private IGroupCellView TryGetCell()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100, _settings.mergingMask))
                return null;
            var cell = hit.collider.gameObject.GetComponent<IGroupCellView>();
            return cell;
        }
        
        private bool TryPurchase(IGroupCellView cell)
        {
            if (cell.IsPurchased)
                return false;
            var cost = cell.Cost;
            if (cost > GC.PlayerData.Money)
            {
                CLog.LogWHeader("MergeInput", $"Not enough money to purchase the cell for {cost}", "w");
                return false;
            }
            GC.PlayerData.Money -= cost;
            cell.Purchase();
            _mergingPage.UpdateMoney();
            return true;
        }

        private void PickFromCell(IGroupCellView cell)
        {
            var item = cell.PickItemView();
            if (item == null)
                return;
            _isActive = true;
            _draggedItem.Init(cell, item);
            _draggedItem.itemView.OnPicked();
        }

        private void PutToCell(IGroupCellView cell)
        {
            if (cell.IsPurchased == false)
            {
                PutItemBack();
                return;
            }
            if (!cell.IsFree)
            {
                if (TryMerge(cell))
                {
                    _draggedItem.SetFree();
                    return;
                }
                Swap(cell);
                return;
            }
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }

        private void Move()
        {
            if (_uiRaycaster.IsMouseOverUI())
            {
                MoveToUI();
                return;
            }
            var ray = _camera.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _settings.groundMask))
                _draggedItem.itemView.SetPosition(hit.point + Vector3.up * _upOffset);
        }

        private void MoveToUI()
        {
            RemoveItemFromGrid(_draggedItem.fromCell);
            _mergeInputUI.TakeItem(_draggedItem.itemView.Item);
            _draggedItem.ClearCellToo();
            _isActive = false;
        }

        private void RemoveItemFromGrid(IGroupCellView cellView)
        {
            GC.ActiveGridSO.GetSetup().ClearCell(cellView.X, cellView.Y);
        }
        
        private bool TryMerge(IGroupCellView cell)
        {
            var item1 = cell.GetItemView().Item;
            var item2 = _draggedItem.itemView.Item;
            var mergedItem = GC.MergeTable.GetMergedItem(item1, item2);
            if (mergedItem == null)
                return false;
            cell.PickItemView().Destroy();
            _draggedItem.itemView.Destroy();
            _mergeItemSpawner.SpawnItem(cell, mergedItem);
            return true;
        }

        private void Swap(IGroupCellView cell)
        {
            var cellItem = cell.PickItemView();
            _draggedItem.fromCell.PutItem(cellItem);
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }
        
        private void PutItemBack()
        {
            if (_draggedItem.IsFree == false)
                return;
            if (!_draggedItem.PutBack())
            {
                Debug.Log($"Cannot put item back!");
            }
            Refresh();
        }

        private void Refresh()
        {
            _draggedItem.itemView.OnReleased();
            _draggedItem.SetFree();
            _isActive = false;
        }

        
        private IGroupCellView GetFreeCell()
        {
            var alLCells  = _gridBuilder.GetSpawnedCells();
            foreach (var row in alLCells)
            {
                for (var x = 0; x < row.Count; x++)
                {
                    var cell = row[x];
                    if (cell.IsFree)
                        return cell;
                }
            }
            return null;
        }
        
        
        private class DraggedItem
        {
            public IGroupCellView fromCell;
            public IMergeItemView itemView;
            private bool _isFree;
            public bool IsFree => _isFree;

            public DraggedItem()
            {
                _isFree = true;
            }
            
            public void Init(IGroupCellView fromCell, IMergeItemView itemView)
            {
                this.itemView = itemView;
                this.fromCell = fromCell;
                _isFree = false;
            }

            public bool PutBack()
            {
                if (fromCell == null || !fromCell.IsFree)
                    return false;
                fromCell.PutItem(itemView);
                _isFree = true;
                return true;
            }

            public void SetFree()
            {
                _isFree = true;
            }

            public void ClearCellToo()
            {
                fromCell.RemoveItem();
                itemView.Destroy();
                SetFree();
            }
            
        }
    }
}