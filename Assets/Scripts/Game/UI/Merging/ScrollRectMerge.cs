using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class ScrollRectMerge : ScrollRect
    {
        public override void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("On begin drag");
            return;
        }
        
        public override void OnDrag(PointerEventData eventData)
        {
            Debug.Log("On drag");
            return;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("On End drag");
            return;
        }
    }
}