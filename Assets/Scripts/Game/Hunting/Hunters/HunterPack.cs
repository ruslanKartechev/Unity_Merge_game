﻿using System;
using System.Collections.Generic;
using Common;
using Common.Utils;
using Game.Hunting.HuntCamera;
using Game.Hunting.Hunters.Interfaces;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HunterPack : MonoBehaviour, IHunterPack
    {
        public event Action OnAllWasted;

        [SerializeField] private HunterAimer _hunterAimer;
        [SerializeField] private HunterBushSpawner _hunterBushSpawner;
        private CamFollower _camFollower;
        private IPreyPack _preyPack;
        private IList<IHunter> _hunters;
        private IList<IHunter> _activeHunters;
        private int _currentHunterIndex;
        private HuntersBush _bush;
        private bool _beganRunning;
        private HunterTargetPicker _targetPicker;
        
        private MovementTracks _tracks;
        private IHunter currentHunter => _activeHunters[_currentHunterIndex];
        
        
        public void Init(IPreyPack preyPack, ProperButton inputButton, CamFollower camFollower, MovementTracks track)
        {
            _tracks = track;
            _preyPack = preyPack;
            _camFollower = camFollower;
            foreach (var hunter in _hunters)
                hunter.CamFollower = _camFollower;
            _hunterAimer.InputButton = inputButton;
            _targetPicker = new HunterTargetPicker(_preyPack);
        }

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
        
        public void IdleState()
        {
            CLog.LogWHeader(nameof(HunterPack), "Idle state", "g", "w");
            foreach (var hunter in _activeHunters)
                hunter.Idle();
            var firstHunter = _hunters[0];
            var preyTarget = _targetPicker.GetBestPrey(firstHunter);
            firstHunter.RotateTo(((MonoBehaviour)preyTarget).transform.position);
            _currentHunterIndex = 0;
            var tr = firstHunter.GetTransform();
            _bush = _hunterBushSpawner.SpawnBush(tr.position, tr.rotation);
        }

        public void AllowAttack()
        {
            _hunterAimer.SetHunter(currentHunter);
            _hunterAimer.Activate();
        }

        public void Win()
        {
            foreach (var hunter in _activeHunters)
                hunter.OnDead -= OnHunterDead;
            foreach (var hunter in _activeHunters)
                hunter.Celebrate();
            _hunterAimer.Stop();
        }

        public float TotalPower()
        {
            var power = 0f;
            foreach (var hunter in _activeHunters)
                power += hunter.Settings.Damage;
            return power;
        }

        public void FocusCamera(bool animated = true)
        {
            var fistHunter = _hunters[0];
            _targetPicker.PickHunterCamTarget(fistHunter, out var target);
            _camFollower.SetTargets(currentHunter.CameraPoint,
                target, 
                !animated);
        }
        
                
        public void BeginChase()
        {
            CLog.LogWHeader(nameof(HunterPack), "Run State", "g", "w");
            _beganRunning = true;
            foreach (var hunter in _activeHunters)
                hunter.Run();
            _bush?.Hide();
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
            if (_targetPicker.PickHunterCamTarget(currentHunter, out var lookTarget))
            {
                _camFollower.SetTargets(currentHunter.CameraPoint, lookTarget);
            }
            else
            {
                // all targets are dead, look in the dir of the hunter
                _camFollower.SimpleFollow(currentHunter.CameraPoint);
            }
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
            _camFollower.SetSingleTarget(_preyPack.CamTarget);
        }

    }
}