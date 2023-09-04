﻿using System;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class HunterPack : MonoBehaviour, IHunterPack
    {
        [SerializeField] private HunterPackMover _mover;
        [SerializeField] private HunterAimer _hunterAimer;
        private CamFollower _camFollower;
        private IPrey _prey;
        private IList<IHunter> _hunters;
        private IList<IHunter> _activeHunters;
        private int _currentHunterIndex;
        public event Action OnAllWasted;


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

        public void SetPrey(IPrey prey)
        {
            _prey = prey;
            _mover.Init(_prey, _activeHunters);
            foreach (var hunter in _activeHunters)
                hunter.SetPrey(prey);
        }

        public void SetCamera(CamFollower camFollower) => _camFollower = camFollower;

        public void Activate()
        {
            CLog.LogWHeader(nameof(HunterPack), "Activated", "g", "w");
            foreach (var hunter in _activeHunters)
                hunter.Run();
            _mover.StartMoving();
            _currentHunterIndex = 0;
            var currentHunter = _activeHunters[_currentHunterIndex];
            _camFollower.SetTargets(currentHunter.GetCameraPoint(), _prey.GetCameraPoint(), true);
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

        private void NextIndex()
        {
            _currentHunterIndex++;
            if (_currentHunterIndex >= _activeHunters.Count)
                _currentHunterIndex = 0;
        }

        private void NextHunter(bool indexUp)
        {
            CLog.LogWHeader("HunterPack", "Next Hunter", "w");
            if(indexUp)
                NextIndex();
            SetupHunter();
        }

        private void SetupHunter()
        {
            var currentHunter = _activeHunters[_currentHunterIndex];
            _camFollower.SetTargets(currentHunter.GetCameraPoint(), _prey.GetCameraPoint(), false);
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
            _camFollower.SetSingleTarget(_prey.GetCameraPoint());
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                NextHunter(true);
        }
        #endif
    }
}