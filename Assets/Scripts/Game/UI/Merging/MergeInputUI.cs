using System.Collections;
using Game.Merging;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeInputUI : MonoBehaviour, IMergeInputUI
    {
        [SerializeField] private UIRaycaster _raycaster;
        [SerializeField] private MergeMovableItemUI _draggedItem;
        [SerializeField] private MergeClassesSwitcher _classesSwitcher;
        private Coroutine _moving;
        
        
        public void Activate()
        {
            _draggedItem.Hide();
            StopInput();
            _moving = StartCoroutine(InputTaking());
        }

        public void Deactivate()
        {
            StopInput();
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
                if (Input.GetMouseButtonDown(0))
                {
                    var itemUI = _raycaster.Cast<IMergeItemUI>();
                    if (itemUI != null && itemUI.Item != null)
                    {
                        _draggedItem.Setup(itemUI);
                        _draggedItem.SetPosition(Input.mousePosition);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_draggedItem.IsActive)
                        OnRelease();
                }
                else if (Input.GetMouseButton(0))
                {
                    if(_draggedItem.IsActive)
                        _draggedItem.SetPosition(Input.mousePosition);
                }
                yield return null;
            }
        }

        private void OnRelease()
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
                _draggedItem.FromCell.SetEmpty();
                _draggedItem.Hide();
                return;
            }
            Merge(itemUI);
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
        }
        
        public void TakeItem(MergeItem item)
        {
            _classesSwitcher.ShowClass(item.class_id);
            GC.ItemsStash.Stash.AddItem(item);
            var cell = _classesSwitcher.CurrentClass.GetFirstFreeCell();
            _draggedItem.SetupNoFromCellEffect(item, cell);
            _draggedItem.SetPosition(Input.mousePosition);
        }
    }
}