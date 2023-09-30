﻿using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class HunterPack : MonoBehaviour, IHunterPack
    {
        public event Action OnAllWasted;

        [SerializeField] private HunterPackMover _mover;
        [SerializeField] private HunterAimer _hunterAimer;
        private CamFollower _camFollower;
        private IPreyPack _prey;
        private IList<IHunter> _hunters;
        private IList<IHunter> _activeHunters;
        private int _currentHunterIndex;
        private bool _beganRunning;

        private IHunter currentHunter => _activeHunters[_currentHunterIndex];
        

        public void SetHunters(IList<IHunter> hunters)
        {
            _hunters = hunters;
            _activeHunters = new List<IHunter>(_hunters.Count);
            foreach (var hh in _hunters)
            {
                _activeHunters.Add(hh);
                hh.OnDead += OnHunterDead;
            }
        }

        public void SetPrey(IPreyPack prey)
        {
            _prey = prey;
            _mover.Init(_prey, _activeHunters);
            foreach (var hh in _hunters)
                hh.SetPrey(prey);
            prey.OnPreyChaseBegin += BeginChase;
        }

        public void IdleState()
        {
            CLog.LogWHeader(nameof(HunterPack), "Idle state", "g", "w");
            _mover.StartMoving();
            foreach (var hunter in _activeHunters)
                hunter.Idle();
            _hunters[0].RotateTo(_prey.Position);
            _currentHunterIndex = 0;
        }

        public void SetCamera(CamFollower camFollower)
        {
            _camFollower = camFollower;
            foreach (var hunter in _hunters)
                hunter.CamFollower = _camFollower;
        }

        public void Activate()
        {
            _hunterAimer.SetHunter(currentHunter);
            _hunterAimer.Activate();
        }

        public void Win()
        {
            foreach (var hunter in _activeHunters)
                hunter.OnDead -= OnHunterDead;
            _hunterAimer.Stop();
            _mover.StopMovement();
            foreach (var hunter in _activeHunters)
                hunter.Celebrate();
        }
        
        public void FocusCamera(bool animated = true)
        {
            _camFollower.SetTargets(currentHunter.GetCameraPoint(), 
                _prey.CamTarget, 
                !animated);
        }
        
                
        private void RunState()
        {
            CLog.LogWHeader(nameof(HunterPack), "Run State", "g", "w");
            foreach (var hunter in _activeHunters)
                hunter.Run();
            _mover.StartMoving();
        }
        
        private void BeginChase()
        {
            _beganRunning = true;
            RunState();
        }

        private void NextIndex()
        {
            _currentHunterIndex++;
            if (_currentHunterIndex >= _activeHunters.Count)
                _currentHunterIndex = 0;
        }

        private void NextHunter(bool indexUp)
        {
            // CLog.LogWHeader("HunterPack", "Next Hunter", "w");
            if(indexUp)
                NextIndex();
            SetActiveHunter();
        }

        private void SetActiveHunter()
        {
            var currentHunter = _activeHunters[_currentHunterIndex];
            _camFollower.SetTargets(currentHunter.GetCameraPoint(),_prey.CamTarget);
            _hunterAimer.SetHunter(currentHunter);
        }
        
        private void OnHunterDead(IHunter hunter)
        {
            if (_activeHunters.Count == 1)
            {
                CLog.LogWHeader("HunterPack", "All hunters dead", "w");
                _activeHunters.Remove(hunter);
                SetCameraToPrey();
                OnAllWasted?.Invoke();
                return;
            }
            _activeHunters.Remove(hunter);
            NextHunter(false);
            _hunterAimer.Activate();
        }

        private void SetCameraToPrey()
        {
            _camFollower.SetSingleTarget(_prey.CamTarget);
        }

    }
}