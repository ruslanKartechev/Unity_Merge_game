using System.Collections;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeInputUI : MonoBehaviour
    {
        [SerializeField] private UIRaycaster _raycaster;
        [SerializeField] private MergeMovableItemUI _draggedItem;
        private Coroutine _moving;
        
        // Debugging
        private void Start()
        {
            Activate();
        }
        //
        
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
                itemUI.ShowItemData();
                _draggedItem.FromCell.SetEmpty();
                _draggedItem.Hide();
                return;
            }
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

    }
}