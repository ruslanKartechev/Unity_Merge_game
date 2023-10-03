using System.Collections;
using System.Runtime.CompilerServices;
using Game.UI;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public partial class MergeInput : MonoBehaviour, IMergeInput
    {
        [SerializeField] private InputSettings _settings;
        [SerializeField] private UIRaycaster _uiRaycaster;
        [SerializeField] private GroupGridBuilder _gridBuilder;
        [SerializeField] private Vector3 _spawnedRotation;
        private IMergeItemSpawner _mergeItemSpawner;
        private DraggedItem _draggedItem;
        private Camera _camera;
        private Vector3 _mousePos;
        private bool _isActive;
        private IMergeStash _mergeStash;
        private Coroutine _inputTaking;
        private Coroutine _moving;

        
        public void SetStash(IMergeStash stash)
        {
            _mergeItemSpawner = gameObject.GetComponent<IMergeItemSpawner>();
            _camera = Camera.main;
            _mergeStash = stash;
            _draggedItem = new DraggedItem();
        }

        public void Activate()
        {
            Deactivate();
            _inputTaking = StartCoroutine(InputTaking());
        }

        public void Deactivate()
        {
            if(_inputTaking != null)
                StopCoroutine(_inputTaking);
        }

        public void TakeItem(MergeItem item)
        {
            // CLog.LogWHeader("Input", $"Item moved to world", "g");
            var view = _mergeItemSpawner.SpawnItem(item);
            view.Item = item;
            view.OnSpawn();
            view.Rotation = Quaternion.Euler(_spawnedRotation);
            var cell = GetFreeCell();
            _draggedItem.Init(cell, view);
            MoveItemToMouse();
            _isActive = true;
            StartMoving();
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
                yield return null;
            }      
        }

        private IEnumerator MovingItem()
        {
            yield return null;
            while (_isActive)
            {
                if (Input.GetMouseButton(0))
                {
                    _mousePos = Input.mousePosition;
                    MoveItemToMouse();
                    if (_uiRaycaster.CheckOverUIMergeArea())
                    {
                        MoveToStashDragging();
                        yield break;
                    }
                }
                yield return null;
            }
        }
        
        private void Click()
        {
            if (!_draggedItem.IsFree) 
                return;
            var cell = GetCellUnderMouse();
            if (cell == null)
                return;
            if (!TryPurchase(cell))
                PickFromCell(cell);
        }
        
        private void Release()
        {
            if (_draggedItem.fromCell == null
                || _draggedItem.fromCell.IsFree == false)
            {
                PutDraggedToStash();
                return;
            }

            var cell = GetCellUnderModel();
            if(cell == null || cell.IsPurchased == false)
                cell = GetCellUnderMouse();
            if (cell != null)
                PutToCell(cell);
            else
                PutItemBack();   
            StopMoving();
        }

        private void StartMoving()
        {
            StopMoving();
            _moving = StartCoroutine(MovingItem());
        }

        private void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }


        private IGroupCellView GetCellUnderMouse()
        {
            return TryGetCell1(Input.mousePosition);
        }

        private IGroupCellView GetCellUnderModel()
        {
            return TryGetCell1(_camera.WorldToScreenPoint(_draggedItem.itemView.GetModelPosition()));
        }
        
        private IGroupCellView TryGetCell1(Vector3 screenPoint)
        {
            var ray = _camera.ScreenPointToRay(screenPoint);
            if (!Physics.Raycast(ray, out var hit, 100, _settings.mergingMask))
                return null;
            var cell = hit.collider.gameObject.GetComponent<IGroupCellView>();
            return cell;
        }

        private void PickFromCell(IGroupCellView cell)
        {
            var item = cell.PickItemView();
            if (item == null)
                return;
            _isActive = true;
            _draggedItem.Init(cell, item);
            _draggedItem.itemView.OnPicked();
            StartMoving();
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
            // NO need to add item to ActiveGroup, item is added when put the CELL. BELOW
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }

        private void MoveItemToMouse()
        {
            var ray = _camera.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _settings.groundMask))
                _draggedItem.itemView.SetDraggedPosition(hit.point + Vector3.up * _settings.draggingUpOffset);
        }

        private void MoveToStashDragging()
        {
            _isActive = false;
            RemoveItemFromGrid(_draggedItem.fromCell);
            _mergeStash.TakeItem(_draggedItem.itemView.Item);
            _draggedItem.ClearCellToo();
        }

        private void RemoveItemFromGrid(IGroupCellView cellView)
        {
            GC.ActiveGroupSO.Group().ClearCell(cellView.X, cellView.Y);
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
            if (_draggedItem.IsFree)
                return;
            if (!_draggedItem.PutBack())
                Debug.LogError($"Error trying to put item BACK!");
            Refresh();
        }

        private void PutDraggedToStash()
        {
            Debug.Log($"Cannot put item back to grid CELL!");
            _mergeStash.TakeToStash(_draggedItem.itemView.Item);
            _draggedItem.ClearDragged();
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
            UIC.UpdateMoney();
            return true;
        }
    }
}