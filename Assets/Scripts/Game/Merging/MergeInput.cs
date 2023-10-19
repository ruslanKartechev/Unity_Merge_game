using System.Collections;
using Game.Levels;
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
        private bool _isMovingItem;
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

        public void TakeFromStash(MergeItem item)
        {
            var view = _mergeItemSpawner.SpawnItem(item);
            view.Item = item;
            view.OnSpawn();
            view.Rotation = Quaternion.Euler(_spawnedRotation);
            // var cell = GetFreeCell();
            _draggedItem.Init(null, view);
            MoveItemToMouse();
            _isMovingItem = true;
            StartMoving();
        }

        private IEnumerator InputTaking()
        {
            while (true)
            {
                if (!_uiRaycaster.CheckOverUIMergeArea() 
                        && GC.Input.IsDown())
                {
                    _mousePos = Input.mousePosition;
                    Click();
                }
                else if (GC.Input.IsUp() && _isMovingItem)
                {
                    Release();
                }
                yield return null;
            }      
        }

        private IEnumerator MovingItem()
        {
            // yield return null;
            while (_isMovingItem)
            {
                if (Input.GetMouseButton(0))
                {
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
            if (_draggedItem.itemView == null
                || _draggedItem.IsFree)
                return;

            var cell = GetCellUnderModel();
            if(cell == null || cell.IsPurchased == false)
                cell = GetCellUnderMouse();
            if (cell != null)
                PutToCell(cell);
            else // if there is no cell
            {
                TryPutBack();
            }
            StopMoving();
        }

        private void TryPutBack()
        {
            if(_draggedItem.fromCell is { IsFree: true })
                PutDraggedBackToCell();
            else
            {
                PutDraggedBackToStash();
            }
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
            _isMovingItem = true;
            _draggedItem.Init(cell, item);
            _draggedItem.itemView.OnPicked();
            StartMoving();
        }

        private void PutToCell(IGroupCellView cell)
        {
            if (cell.IsPurchased == false)
            {
                PutDraggedItemBack();
                return;
            }
            if (!cell.IsFree)
            {
                if (TryMerge(cell))
                {
                    _draggedItem.SetFree();
                    return;
                }
                
                if(_draggedItem.fromCell != null) // if item was picked up
                    Swap(cell);
                else
                    SwapAndMoveToStash(cell); // if item was taken from stash
                return;
            }
            // NO need to add item to ActiveGroup, item is added when put the CELL. BELOW
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }

        private void PutDraggedItemBack()
        {
            if (_draggedItem.fromCell != null && _draggedItem.fromCell.IsFree)
            {
                var cell = GetFreeCell();
                if (cell != null)
                {
                    _draggedItem.fromCell = cell;
                    PutDraggedBackToCell();
                    return;        
                }
            }
            PutDraggedBackToStash();            
        }

        private void MoveItemToMouse()
        {
            _mousePos = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _settings.groundMask))
                _draggedItem.itemView.SetDraggedPosition(hit.point + Vector3.up * _settings.draggingUpOffset);
        }

        private void MoveToStashDragging()
        {
            _isMovingItem = false;
            if(_draggedItem.fromCell != null)
                RemoveItemFromGrid(_draggedItem.fromCell);
            _mergeStash.TakeItem(_draggedItem.itemView.Item);
            _draggedItem.ClearCellToo();
            StopMoving();
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
            Debug.Log("Swap");
            var cellItem = cell.PickItemView();
            _draggedItem.fromCell.PutItem(cellItem);
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }
        
        private void PutDraggedBackToCell()
        {
            if (_draggedItem.IsFree)
                return;
            if (!_draggedItem.PutBack())
                Debug.LogError($"Error trying to put item BACK!");
            Refresh();
        }

        private void SwapAndMoveToStash(IGroupCellView cell)
        {
            Debug.Log($"Swapping with dragged item and moving grid item to stash");
            MoveFromCellToStash(cell);
            ClearCellCompletely(cell);
            PutToCellFromDragged(cell);
            Refresh();
        }

        private void PutDraggedBackToStash()
        {
            _mergeStash.TakeToStash(_draggedItem.itemView.Item);
            _draggedItem.ClearDragged();
        }

        private void MoveFromCellToStash(IGroupCellView cell)
        {
            var cellItem = cell.GetItem();
            _mergeStash.TakeToStash(cellItem);   
        }
        
        private void PutToCellFromDragged(IGroupCellView cell)
        {
            var spawnedItem = _draggedItem.itemView.Item;
            _mergeItemSpawner.SpawnItem(cell, spawnedItem);
            _draggedItem.ClearDragged();
        }

        private void ClearCellCompletely(IGroupCellView cell)
        {
            var view = cell.GetItemView();
            view.Destroy();
            cell.RemoveItem();
        }
        
        private void Refresh()
        {
            _draggedItem.itemView.OnReleased();
            _draggedItem.SetFree();
            _isMovingItem = false;
        }

        private IGroupCellView GetFreeCell()
        {
            var alLCells  = _gridBuilder.GetSpawnedCells();
            foreach (var row in alLCells)
            {
                for (var x = 0; x < row.Count; x++)
                {
                    var cell = row[x];
                    if (cell.IsFree && cell.IsPurchased)
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
            AnalyticsEvents.OnGridCellPurchase(cell.X, cell.Y, cell.Cost);
            return true;
        }
    }
}