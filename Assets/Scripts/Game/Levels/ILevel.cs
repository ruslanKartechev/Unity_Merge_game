using System;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.UI.Hunting;

namespace Game.Levels
{
    public interface ILevel
    {
        public event Action OnContinue;
        public event Action OnReplay;
        public event Action OnExit;
        void Init(IHuntUIPage ui, MovementTracks track, CamFollower camera);
        
    }
    
    
}