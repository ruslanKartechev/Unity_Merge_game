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
        private IMergingPage _mergingPage;
        private IMergeItemSpawner _mergeItemSpawner;
        private Coroutine _inputTaking;
        private DraggedItem _draggedItem;
        private Camera _camera;
        private Vector3 _mousePos;
        private bool _active;
        private IMergeInputUI _mergeInputUI;
        
        
        public void Init(IMergingPage page, IMergeItemSpawner spawner, IMergeInputUI mergeInputUI)
        {
            _mergingPage = page;
            _mergeItemSpawner = spawner;
            _camera = Camera.main;
            _mergeInputUI = mergeInputUI;
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

        public void Click()
        {
            var cell = TryGetCell();
            if (cell == null)
                return;
            if (_draggedItem == null)
            {
                if (!TryPurchase(cell))
                    PickFromCell(cell);
                
            }
        }

        public void TakeItem(MergeItem item)
        {
            Debug.Log("called to spawn item");
            PutItemBack();
            var instance = GC.ItemViewRepository.GetPrefab(item.item_id);
            var view = instance.GetComponent<IMergeItemView>();
            view.Item = item;
            _draggedItem = new DraggedItem(null, view);
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
                    Click();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_active)
                        Release();
                }
                if (Input.GetMouseButton(0))
                {
                    _mousePos = newPos = Input.mousePosition;
                    if (_active)
                        Move();
                }

                oldPos = newPos;
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
        
        private IGroupCell TryGetCell()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100, _settings.mergingMask))
                return null;
            var cell = hit.collider.gameObject.GetComponent<IGroupCell>();
            return cell;
        }
        
        private bool TryPurchase(IGroupCell cell)
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

        private void PickFromCell(IGroupCell cell)
        {
            var item = cell.PickItemView();
            if (item == null)
                return;
            _active = true;
            _draggedItem = new DraggedItem(cell, item);
            _draggedItem.itemView.OnPicked();
        }

        private void PutToCell(IGroupCell cell)
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
                    _draggedItem = null;
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
                Debug.Log("IS OVER UI");
                _mergeInputUI.TakeItem(_draggedItem.itemView.Item);
                
                return;
            }
            var ray = _camera.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _settings.groundMask))
                _draggedItem.itemView.SetPosition(hit.point + Vector3.up * _upOffset);
        }

        private bool TryMerge(IGroupCell cell)
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

        private void Swap(IGroupCell cell)
        {
            var cellItem = cell.PickItemView();
            _draggedItem.fromCell.PutItem(cellItem);
            cell.PutItem(_draggedItem.itemView);
            Refresh();
        }
        
        private void PutItemBack()
        {
            if (_draggedItem == null)
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
            _draggedItem = null;
            _active = false;
        }

        private class DraggedItem
        {
            public IGroupCell fromCell;
            public IMergeItemView itemView;
            
            public DraggedItem(IGroupCell fromCell, IMergeItemView itemView)
            {
                this.itemView = itemView;
                this.fromCell = fromCell;
            }

            public bool PutBack()
            {
                if (fromCell == null || !fromCell.IsFree)
                    return false;
                fromCell.PutItem(itemView);
                return true;
            }
        }
    }
}