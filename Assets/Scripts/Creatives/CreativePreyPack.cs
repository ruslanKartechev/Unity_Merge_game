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
        
        [SerializeField] private CreativeCar _car;
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
            _prey.Add(_car);
            _car.Init();
        }

        public void Idle()
        { }

        public void RunAttacked()
        {
            _preyPackMover.BeginMoving();
            _car.OnPackRun();
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