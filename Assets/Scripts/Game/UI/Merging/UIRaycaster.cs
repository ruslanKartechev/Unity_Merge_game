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
        private PointerEventData _pointerEventData;

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

        public bool IsMouseOverUI()
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            return results.Count > 0;
        }
    }
    
}