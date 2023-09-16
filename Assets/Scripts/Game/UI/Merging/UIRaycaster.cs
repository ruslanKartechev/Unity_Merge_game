using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class UIRaycaster : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private string _tag;
        private PointerEventData _pointerEventData;

        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_eventSystem == null)
                _eventSystem = FindObjectOfType<EventSystem>();
        }
#endif
        
        public T Cast<T>()
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            foreach (var result in results)
            {
                var comp = result.gameObject.GetComponent<T>();
                if(comp != null)
                    return comp;
            }
            return default;
        }

        public bool CheckOverUIMergeArea()
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            // Debug.Log($"UI Raycast: {results.Count}");
            foreach (var result in results)
            {
                if (result.gameObject.CompareTag(_tag))
                    return true;
            }
            return false;
        }
    }
    
}