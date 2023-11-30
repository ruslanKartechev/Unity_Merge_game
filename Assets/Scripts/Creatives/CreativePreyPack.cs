using System;
using System.Collections.Generic;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using Game.Levels;
using UnityEngine;

namespace Creatives
{
    public class CreativePreyPack : MonoBehaviour, IPreyPack
    {
        public event Action OnAllDead;
        public event Action<IPrey> OnPreyKilled;
        public event Action OnBeganMoving;
        
        [SerializeField] private GameObject _enemy;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private PreyPackMover _preyPackMover;
        
        public int PreyCount { get; }
        public ICamFollowTarget CamTarget => _camFollowTarget;
        
        private HashSet<IPrey> _prey;
        public HashSet<IPrey> GetPrey()
        {
            return _prey;
        }

        public void Init(MovementTracks track, ILevelSettings levelSettings)
        {
            _preyPackMover.Init(track);
            _prey = new HashSet<IPrey>();
            if (_enemy.TryGetComponent<CreativeCar>( out var car))
            {
                _prey.Add(car);
                car.Init();         
            }
            else if(_enemy.TryGetComponent<CreativeBoat>( out var boat))
            {
                _prey.Add(boat);
                boat.Init();
            }
        }

        public void Idle()
        { }

        public void RunAttacked()
        {
            _preyPackMover.BeginMoving();
            foreach (var prey in _prey)
                prey.OnPackRun();
        }

        public void RunCameraAround(GameObject cam, Action returnCamera)
        {
            returnCamera?.Invoke();
        }

        public float TotalPower()
        {
            return 100;
        }
    }
}