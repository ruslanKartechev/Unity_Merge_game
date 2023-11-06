using System;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterAnimEventReceiver : MonoBehaviour
    {
        public event Action OnJumpEvent;
        
        public void OnJumpAnimation()
        {
            OnJumpEvent?.Invoke();
        }
        
    }
}