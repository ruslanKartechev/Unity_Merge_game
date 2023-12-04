using System;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongThrowerEventCatcher : MonoBehaviour
    {
        public event Action OnGrabEvent;
        public event Action OnThrowEvent;        
        public void OnGrab()
        {
            OnGrabEvent?.Invoke();
        }

        public void OnThrow()
        {
            OnThrowEvent?.Invoke();
        }
        
    }
}