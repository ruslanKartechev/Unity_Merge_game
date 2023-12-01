using System;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey.Interfaces;
using Game.Levels;
using Game.UI.Hunting;
using UnityEngine;

namespace Creatives
{
    public class CreativesLevel : MonoBehaviour, ILevel
    {
        public event Action OnContinue;
        public event Action OnReplay;
        public event Action OnExit;
        
        [SerializeField] private CreativesHunterPack _hunters;
        [SerializeField] private GameObject _prePackGo;
        private IPreyPack _preyPack;
        

        public void Init(IHuntUIPage ui, MovementTracks track, GameObject camera)
        {
            var prey = _prePackGo.GetComponent<IPreyPack>();
            _preyPack = prey;
            _preyPack.Init(track, null);
            _hunters.Init(prey, ui.InputButton, camera, track);
            // _hunters.IdleState();
            _hunters.FocusCamera(false);
            Game.Core.GC.Input.Enable();
            _hunters.AllowAttack();    
            _preyPack.RunAttacked();
            _hunters.BeginChase();
        }
    }
}