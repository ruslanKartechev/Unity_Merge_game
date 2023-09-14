using System;
using System.Collections;
using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeInputUI : MonoBehaviour
    {
        [SerializeField] private UIRaycaster _raycaster;
        [SerializeField] private MergeMovableItemUI _movable;
        private Coroutine _moving;

        
        // Debugging
        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            _movable.Hide();
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
                        _movable.Setup(itemUI);
                        _movable.SetPosition(Input.mousePosition);
                    }
                    else
                        Debug.Log($"Null raycasted");
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_movable.IsActive)
                        OnRelease();
                }
                else if (Input.GetMouseButton(0))
                {
                    if(_movable.IsActive)
                        _movable.SetPosition(Input.mousePosition);
                }
                yield return null;
            }
        }

        private void OnRelease()
        {
            var itemUI = _raycaster.Cast<IMergeItemUI>();
            if (itemUI == null) 
                return;
            if (itemUI.Item == null)
            {
                itemUI.Item = _movable.FromCell.Item;
                itemUI.ShowItemData();
                _movable.FromCell.SetEmpty();
                _movable.Hide();
                return;
            }
            var table = GC.MergeTable;
            var merged = table.GetMergedItem(itemUI.Item, _movable.FromCell.Item);
            if (merged == null)
                _movable.SetBack();
            else
            {
                _movable.FromCell.SetEmpty();
                itemUI.SetMerged(merged);
                _movable.Hide();
            }
        }

    }
}