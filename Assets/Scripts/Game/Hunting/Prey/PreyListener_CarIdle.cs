using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListener_CarIdle : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        
        public void Begin()
        {
        }

        public void Stop()
        {
        }
    }
}