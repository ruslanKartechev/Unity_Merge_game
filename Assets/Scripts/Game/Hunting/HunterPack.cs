using System;
using System.Collections.Generic;
using Common;
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
        [SerializeField] private HunterBushSpawner _hunterBushSpawner;
        private CamFollower _camFollower;
        private IPreyPack _preyPack;
        private IList<IHunter> _hunters;
        private IList<IHunter> _activeHunters;
        private int _currentHunterIndex;
        private HuntersBush _bush;
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
        
        public void IdleState()
        {
            CLog.LogWHeader(nameof(HunterPack), "Idle state", "g", "w");
            foreach (var hunter in _activeHunters)
                hunter.Idle();
            _hunters[0].RotateTo(_preyPack.Position);
            _currentHunterIndex = 0;
            var tr = _hunters[0].GetTransform();
            _bush = _hunterBushSpawner.SpawnBush(tr.position, tr.rotation);
        }

        public void Init(IPreyPack preyPack, ProperButton inputButton, CamFollower camFollower, MovementTracks track)
        {
            _preyPack = preyPack;
            // _mover.Init(_preyPack, _activeHunters);
            foreach (var hh in _hunters)
                hh.SetPrey(preyPack);
            _camFollower = camFollower;
            foreach (var hunter in _hunters)
                hunter.CamFollower = _camFollower;
            _hunterAimer.InputButton = inputButton;
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
            _camFollower.SetTargets(currentHunter.GetCameraPoint(), 
                _preyPack.CamTarget, 
                !animated);
        }
        
                
        public void BeginChase()
        {
            CLog.LogWHeader(nameof(HunterPack), "Run State", "g", "w");
            _beganRunning = true;
            foreach (var hunter in _activeHunters)
                hunter.Run();
            _bush.Hide();
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
            _camFollower.SetTargets(currentHunter.GetCameraPoint(),_preyPack.CamTarget);
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