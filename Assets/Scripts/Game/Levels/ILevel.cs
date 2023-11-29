using System;
using Game.Hunting;
using Game.UI.Hunting;
using UnityEngine;

namespace Game.Levels
{
    public interface ILevel
    {
        public event Action OnContinue;
        public event Action OnReplay;
        public event Action OnExit;
        void Init(IHuntUIPage ui, MovementTracks track, GameObject camera);
        
    }
    
    
}