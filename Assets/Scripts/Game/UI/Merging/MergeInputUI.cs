using System.Collections;
using Common.Utils;
using Game.Core;
using Game.Merging;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeInputUI : MonoBehaviour, IMergeStash
    {
        [SerializeField] private UIRaycaster _raycaster;
        [SerializeField] private MergeMovableItemUI _draggedItem;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;
        private IMergeInput _mergeInput;
        private Coroutine _moving;

        public void SetInput(IMergeInput input)
        {
            _mergeInput = input;
        }
        
        public void Activate()
        {
            CLog.LogWHeader("UI Input", "Activated", "w");
            _mergeInput.Activate();
            _draggedItem.Hide();
            StopInput();
            _moving = StartCoroutine(InputTaking());
        }

        public void Deactivate()
        {
            CLog.LogWHeader("UI Input", "Deactivated", "w");
            _mergeInput.Deactivate();
            StopInput();
        }
        
        public void TakeItem(MergeItem item)
        {
            // CLog.LogWHeader("UI", $"Item moved to UI", "y");
            if (_draggedItem.IsHidden && _draggedItem.FromCell.Item == item)
            {
                // CLog.LogWHeader("UI", $"Returned the same item", "y");
                _draggedItem.ShowAsPrevious();
                _draggedItem.SetPosition(Input.mousePosition);
                AddToStash(item);
                return;
            }
            _classesSwitcher.ShowClass(item.class_id);
            var cell = _classesSwitcher.CurrentClass.GetFirstFreeCell();
            _draggedItem.SetupNoFromCellEffect(item, cell);
            _draggedItem.SetPosition(Input.mousePosition);
            AddToStash(item);
        }

        public void TakeToStash(MergeItem item)
        {
            _classesSwitcher.ShowClass(item.class_id);
            var cell = _classesSwitcher.CurrentClass.GetFirstFreeCell();
            cell.Item = item;
            cell.ShowItemView();
            cell.PlayItemSet();
            cell.SetDarkened(false);
            AddToStash(item);      
        }

        private void AddToStash(MergeItem item)
        {
            GC.ItemsStash.Stash.AddItem(item);
            _classesSwitcher.UpdateCounts();
        }

        private void RemoveFromStash(MergeItem item)
        {
            GC.ItemsStash.Stash.RemoveItem(item);
        }
        
        private void StopInput()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }

        private IEnumerator InputTaking()
        {
            while (true)
            {
                if (GC.Input.IsDown())
                    OnClick();
                else if (GC.Input.IsUp())
                    OnRelease();
                else if (GC.Input.IsPressed())
                {
                    if (_draggedItem.IsActive)
                        MoveDragged();
                }
                yield return null;
            }
        }

        private void OnClick()
        {
            var itemUI = _raycaster.Cast<IMergeItemUI>();
            if (itemUI != null && itemUI.Item != null)
            {
                _draggedItem.Setup(itemUI);
                _draggedItem.SetPosition(Input.mousePosition);
            }
        }

        private void OnRelease()
        {
            if (_draggedItem.IsActive)
                TryPutItem();
            else if (_draggedItem.IsHidden)
                ClearFromCell();
        }
        
        private void TryPutItem()
        {
            var itemUI = _raycaster.Cast<IMergeItemUI>();
            if (itemUI == null || itemUI == _draggedItem.FromCell)
            {
                _draggedItem.SetBack();
                return;
            }
            if (itemUI.Item == null)
            {
                itemUI.Item = _draggedItem.FromCell.Item;
                itemUI.ShowItemView();
                ClearFromCell();
                return;
            }
            Merge(itemUI);
        }

        private void MoveDragged()
        {
            _draggedItem.SetPosition(Input.mousePosition);
            if (_raycaster.CheckOverUIMergeArea() == false)
            {
                _mergeInput.TakeFromStash(_draggedItem.Item);
                _draggedItem.HideView();
                RemoveFromStash(_draggedItem.Item);
            }          
        }

        private void ClearFromCell()
        {
            _draggedItem.FromCell.SetEmpty();
            _draggedItem.Hide();
            _classesSwitcher.UpdateCounts();
        }

        private void Merge(IMergeItemUI itemUI)
        {
            var merged = GC.MergeTable.GetMergedItem(itemUI.Item, _draggedItem.FromCell.Item);
            if (merged == null)
                _draggedItem.SetBack();
            else
            {
                var stashItemsClass = GC.ItemsStash.Stash.GetClass(merged.class_id);
                stashItemsClass.items.Remove(_draggedItem.FromCell.Item);
                stashItemsClass.items.Remove(itemUI.Item);
                stashItemsClass.items.Add(merged);
                _draggedItem.FromCell.SetEmpty();
                itemUI.SetMerged(merged);
                // Debug.Log($"MERGED Item id: {merged.item_id},  Sprite name: {merged.sprite}");
                _draggedItem.Hide();
            }   
            _classesSwitcher.UpdateCounts();
        }
        
  
    }
}